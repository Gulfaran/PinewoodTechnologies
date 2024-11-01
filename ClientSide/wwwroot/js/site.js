﻿
// the base endpoint of the API to call for this page
const api = 'http://localhost:5159/api/customers'
$(document).ready(function () {

    // route the submittion event to the submitted function
    var form = document.querySelector("form");
    form.onsubmit = submitted.bind(form);

    //populate the customer table on load
    getData();

});

function getData(p, e, s) {
    var callapi = api;
    if (p) {
        callapi += '/' + p + '/' + e + '/' + s
    }
    $.get(callapi, function (data) {
        // Call the function used to populate the table dynamically based on the information in the data returned
        debugger
        addTable(data,'CustomerList','id')
    }, 'json');
}

function getList(table) {
    var s = table.attr('data-order')
    var e = table.attr('data-entries')
    var p = table.attr('data-page')

    if (!s) {
        s = '';
    }

    if (!p) {
        p=1
    }

    if (!e) {
        e=10
    }

    getData(p, e, s)
}

// Take an data object, table ID of the table to populate and an actionid name if applicable used in populating the id for edit and delete events
function addTable(data, id, actionidname) {
    var table = $('#' + id);
    var noofpages = data.noofpages;

    if ($('#' + id + 'EntriesPerPage').length==0) {
        var dropdown = $('<select></select>').attr('id', id + 'EntriesPerPage');
        // Add options to the dropdown
        var options = [10, 30, 50];
        $.each(options, function (index, value) {
            dropdown.append($('<option></option>').attr('value', value).text(value));
        });
        table.after(dropdown)
        table.attr('data-entries', 10)
        dropdown.change(function () {
            table.attr('data-entries', $(this).val())
            getList(table)
        });

        dropdown.before('<span class="tbl_entries_lb">Show</span>')
        dropdown.after('<span class="tbl_entries_lb">per page</span>')
    }

    var pagedropdown = $('#' + id + 'Page');

    if (pagedropdown.length == 0) {
        pagedropdown = $('<select></select>').attr('id', id + 'Page');
        // Add options to the dropdown
        table.after(pagedropdown)
        table.attr('data-page',1)
        pagedropdown.change(function () {
            table.attr('data-page', $(this).val())
            getList(table)
        });
        pagedropdown.before('<span class="tbl_page_lb">Page</span>')
        pagedropdown.after('<span class="tbl_page_lb" id="totalpages"></span>')
    }

    pagedropdown.empty()
    for (var i = 1; i <= noofpages; i++) {
        pagedropdown.append($('<option></option>').attr('value', i).text(i));
    }
    var selected = table.attr('data-page')

    $('#totalpages').text('of ' + noofpages)

    if (selected > noofpages) {
        selected = noofpages
    }

    pagedropdown.val(selected);
    // empty the table first
    table.html('')

    var tabledata = data.data;

    // as long as there is data to be processed
    if (tabledata.length > 0) {
        var row = '<tr>';

        // add a row for the headings could be further updated to use prettier names than the variable name
        // also sort functions can be binded at this point too
        $.each(tabledata[0], function (name, value) {
            if (name == data.defaultorder) {
                row += '<th class="tbl_sort_slct">' + name + '</th>'
            } else { 
                row += '<th>' + name + '</th>'
            }
        });
        row += '</tr>'
        table.append(row)

        table.attr('data-order', data.defaultorder)
        $('#' + id + ' th').on('click', function () {            
            table.attr('data-order', $(this).text())
            getList(table)
        });

        // add the rows containing actual information
        $.each(tabledata, function (i, v) {
            var drow = '<tr>';
            var id = 0;
            $.each(v, function (name, value) {
                drow += '<td>' + value + '</td>'
                // if the actionname was given and the field name matches the name remeber the id
                if (name == actionidname) {
                    id = value
                }
            });
            // add the edit and delete button for the id that was found
            if (id > 0) {
                drow += '<td><button onclick="viewedit(' + id + ')" type="button" >View/Edit</button></td><td><button onclick="deleteCustomer(' + id +')" type="button">Delete</button></td>'
            }
            drow += '</tr>'
            table.append(drow)
        });
    } else {
        $('#'+id).append('<tr><td>No data found<td></tr>');
    }
}

// Edit functionality
function viewedit(id) {
    $.get(api + '/' + id).done(function (data) {
        // use the data to populate the relevant form
        populateform(data, 'customer')
    }).fail(function (xhr, ajaxOptions, thrownError) {
        errorHandling(xhr, ajaxOptions, thrownError)
    });
}

// based on naming conventions find and populate the ids
function populateform(data,parentname) {
    $.each(data, function (name, value) {
        var namelookup = name

        // handle nested objects
        if (typeof value === 'object') {
            populateform(value, parentname + '.'+name)
        } 

        var idLookup = parentname + '.' + namelookup;
        idLookup = idLookup.replaceAll('.', '_')
        var found = $("*[id]").filter(function () {
            return $(this).attr("id").toLowerCase() === idLookup; // Convert the element's ID to lowercase and compare to prevent capitalisation errors
        })
        found.val(value);

        // clear valdation messages from previous submissions
        $("[data-valmsg-for='" + found.attr('name') + "']").text('')
    });
}

// Delete functionality
function deleteCustomer(id) {
    // simple ajax call to request the deletion of the record
    $.ajax({
        type: "DELETE",
        url: api + '/' + id,
        success: function () {
            getList($('#CustomerList'));
            setResultMessage("Record removed")
        },
        error: function (xhr, ajaxOptions, thrownError) {
            errorHandling(xhr, ajaxOptions, thrownError)
        },
        dataType: "json",
        contentType: "application/json"
    })
}

function submitted(event) {

    event.preventDefault();

    var form = $(this)

    // take the form data and put it into a format that can be read by json
    var formData = convertFormToJSON(form);

    // get a jquery object so we can review the status of the id field
    var data = JSON.parse(formData)

    // if there is no id this is a new record otherwise we are updating an existing record
    var submitType = data.Id > 0 ? 'PUT' : 'POST'

    $.ajax({
        type: submitType,
        url: data.Id > 0 ? api + '/' + data.Id : api,
        data: formData,
        dataType: "json",
        contentType: "application/json",
        success: function () {
            getList($('#CustomerList')); form[0].reset(), setResultMessage("Record has been successfuly saved")
        },
        error: function (xhr, ajaxOptions, thrownError) {
            errorHandling(xhr, ajaxOptions, thrownError, form)
        }
    })

}

// if an error occurs on any submission route the information as necessary
function errorHandling(jqXHR, textStatus, errorThrown, form) {
    if (jqXHR.responseJSON.errors) {
        var errors = jqXHR.responseJSON.errors;

        if (form) {
            $.each(errors, function (name, value) {
                form.find("[data-valmsg-for*='" + name + "']").text(value[0])
            });
        } else {
            var msg = ''
            $.each(errors, function (name, value) {
                msg += '*' + name + ' ' + value
            });
            setResultMessage(msg)
        }
    } else {
        setResultMessage("Unfortunately something has gone wrong")
    }
}

// Summarised information of the result of a customer action can be given here
function setResultMessage(msg) {
    $('#resultsummary').text(msg)
}

// Clear the form and remove all validation messages
function ClearForm(e) {
    var form = $(e).closest('form');
    form.find("[data-valmsg-for]").each(function (i, e) {
        $(e).text('')
    })
    form[0].reset()
}

// When a form is submitted again remove any existing validation messages
$('form').on('submit', function (e) {
    $(e.currentTarget).find("[data-valmsg-for]").each(function (i, e) {
        $(e).text('')
    })
    setResultMessage('')
});

// when reseting a form hidden values are left as is, to prevent existing records being updated when new should be created we clear the hidden values
$('form').on('reset', function () {
    $("input[type='hidden']", $(this)).val('');
});

// Due to the way form data is set up nested values are unreadable for json
// this will process the data so that values and the names of the values are
// nested into objects instead of a one single object
function convertFormToJSON(form) {
    let formData = $(form).serializeArray();
    let customer = {};

    // Function to set nested object values
    function setNestedValue(obj, path, value) {
        let keys = path.split('.').slice(1);
        let lastKey = keys.pop();
        let deepObj = keys.reduce((acc, key) => acc[key] = acc[key] || {}, obj);
        deepObj[lastKey] = value;
        debugger
    }

    // Convert form data to nested JSON object
    $.each(formData, function (index, field) {
        setNestedValue(customer, field.name, field.value);
    });

    return JSON.stringify(customer)
}
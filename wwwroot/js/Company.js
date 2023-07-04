// Initialize the Typeahead input field with the dummy array as the data source
$(document).ready(function () {
   

   // var people = loadContacts(handleContacts);

});

/* Load Person information into typeahead Edit page*/
function loadContacts(callback) {
    $.ajax({
        type: "GET",
        url: "/Accountant/Companies/GetAllPerson",
        success: function (result) {
            var people = result.data.map(function (person) {
                return { name: person.name, email: person.email }
            })
            callback(people);
        },
        error: function (xhr, status, error) {
            toastr["error"](error, status.toString());
            callback([]);
        }
    })

}
/* Get Form and add Bootstrap validation */
/*var form = document.getElementById('formEdit')
form.addEventListener('submit', function (event) {
    if (!form.checkValidity()) {
        event.preventDefault()
        event.stopPropagation()
        form.classList.add("was-validated")        
    }
    else {
        form.submit();
    }
});*/

// Define a dummy array for the data source
var states = ['Alabama', 'Alaska', 'Arizona', 'Arkansas', 'California', 'Colorado', 'Connecticut', 'Delaware', 'Florida', 'Georgia', 'Hawaii', 'Idaho', 'Illinois', 'Indiana', 'Iowa', 'Kansas', 'Kentucky', 'Louisiana', 'Maine', 'Maryland', 'Massachusetts', 'Michigan', 'Minnesota', 'Mississippi', 'Missouri', 'Montana', 'Nebraska', 'Nevada', 'New Hampshire', 'New Jersey', 'New Mexico', 'New York', 'North Carolina', 'North Dakota', 'Ohio', 'Oklahoma', 'Oregon', 'Pennsylvania', 'Rhode Island', 'South Carolina', 'South Dakota', 'Tennessee', 'Texas', 'Utah', 'Vermont', 'Virginia', 'Washington', 'West Virginia', 'Wisconsin', 'Wyoming'];
function handleContacts(people) {
    $('.typeahead').typeahead({
        source: people
    });
}



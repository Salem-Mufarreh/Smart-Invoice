function myfunction(company) {
    $.ajax({
        url: "/Accountant/Invoices/CheckCompany?companyName=" + encodeURIComponent(company),
        type: "GET",
        success: function (data) {
            if (data == true) {
                toastr["success"]("good")
            }
            else {
                $("#myModal").modal("show");


            }
        }

    })
}
var form = document.getElementById("EditForm");
form.addEventListener('submit', function (event) {
    if (!form.checkValidity()) {
        event.preventDefault()
        event.stopPropagation()
        form.classList.add("was-validated")
    }
    else {
        form.submit();
    }
});
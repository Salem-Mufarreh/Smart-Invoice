
function myfunction2(company) {
    $.ajax({
        url: "/Accountant/Products/AddProductPopup?itemId=" + encodeURIComponent(company),
        type: "GET",
        success: function (data) {
            if (data != null) {
                $("#myModal .modal-body").html(data);
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

document.addEventListener('submit', function (event) {
    var form = event.target.closest("#CreateProductPartial");
    if (form) {
        if (!form.checkValidity()) {
            event.preventDefault()
            event.stopPropagation()
            form.classList.add("was-validated")
        }
        else {
            event.preventDefault();
            event.stopPropagation();
            var formData = $("#CreateProductPartial").serializeArray();
            var convertedData = {};

            var formDataObj = Object.fromEntries(new FormData(event.target).entries());
            var x = JSON.stringify(formDataObj);
            console.log(x);
            formData.forEach(function (field) {
                switch (field.name) {
                    case "Price":
                    case "CostPrice":
                        convertedData[field.name] = parseFloat(field.value);
                        break;
                    case "IsAvailable":
                    case "IsActive":
                        convertedData[field.name] = field.value === "true";
                        break;
                    default:
                        convertedData[field.name] = field.value;
                }
            });
            
            $.ajax({
                url: "/Accountant/Products/SubmitProduct",
                method: "POST",
                contentType: "application/json",
                data: JSON.stringify(convertedData),
                success: function (data) {
                    $("#myModal").modal("hide");
                    var select = document.getElementById("Product " + usingItem);
                    var option = document.createElement("option");
                    var button = $(".add-product-btn");
                    button.hide();
                    var parentDiv = select.parentNode;
                    parentDiv.classList.remove("col-md-6");
                    parentDiv.style.marginRight = 0;
                    option.text = data.data;
                    option.value = data.data;
                    option.selected = true;
                    option.setAttribute("selected", "selected");
                    select.add(option);
                    var selectedOption = select.options[select.selectedIndex];
                    select.remove(selectedOption);
                    select.classList.remove("border-danger");
                    select.classList.add("border-success","is-valid");
                    var span = document.getElementById("NotFound " + usingItem);
                    span.textContent = "product Has been Create";
                    span.classList.remove("text-danger");
                    span.classList.add("text-success");
                    select.style.pointerEvents = 'none';
                    toastr["success"]("Product has been Created !");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Status: " + XMLHttpRequest);

                }
            });
        }
       
    }
});
function closePopUp() {
    $("#myModal").modal("hide");

}
var usingItem;
$(document).ready(function () {
    $(".add-product-btn").click(function (event) {
        event.preventDefault(); // Prevent form submission
        var itemId = $(this).data("item-id");
        usingItem = itemId;
        myfunction2(itemId);
    });
    
});


/*
$(document).ready(function () {
    $(".add-product-btn").click(function () {
        var itemId = $(this).data("item-id");

        // Send an AJAX request to fetch the partial view content
        $.get("/Accountant/Product/AddProductPopup?itemId=" + itemId, function (response) {
            // Set the content of the popup
            $("#add-product-popup").html(response);

            // Show the popup
            $("#add-product-popup").show();
        });
    });
});*/
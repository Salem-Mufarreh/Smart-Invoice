
var tableIndex = 2 ;
function myfunction2(productName) {
    $.ajax({
        url: "/Accountant/Products/AddProductPopup?itemId=" + encodeURIComponent(productName),
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
    var form = event.target.closest("#CreateCompanyPartial");
    if (form) {
        if (!form.checkValidity()) {
            event.preventDefault()
            event.stopPropagation()
            form.classList.add("was-validated")
        }
        else {
            event.preventDefault();
            event.stopPropagation();
            var formData = $("#CreateCompanyPartial").serializeArray();
            var convertedData = {};

            var formDataObj = Object.fromEntries(new FormData(event.target).entries());
            var x = JSON.stringify(formDataObj);
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
                url: "/Accountant/Companies/SubmitCompany",
                method: "POST",
                contentType: "application/json",
                data: JSON.stringify(formDataObj),
                success: function (data) {
                    $("#CreateCompany").modal("hide");
                    document.getElementById("Company_Name").classList.add("is-valid");
                    $(".add-company-btn").hide();
                    toastr["success"]("Company was Created !");
                }
            });
        }

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
                    var button = $("#" + usingItem);
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
function closePopUp(clickedButton) {
    $(clickedButton).closest(".modal").modal("hide");

}
var usingItem;
$(document).ready(function () {
    $(".add-product-btn").click(function (event) {
        event.preventDefault(); // Prevent form submission
        
        var buttonId = $(this).attr("id");
        usingItem = buttonId;
        myfunction2(buttonId);
    }),
    $(".add-company-btn").click(function (event) {
        event.preventDefault();
        InitCompany();
    }),
    $("#btn-addline").click(function (event) {
        event.preventDefault();
        GenerateItemBlock();
        setTimeout(function () {
            $(".typeahead").typeahead({
                source: CommonProducts,
            });
        }, 500);
        $(".remove-product-btn").click(function (event) {
            event.preventDefault();
            var table = $('#example').DataTable();
            var blockId = $(this).data("item-id");
            deleteRow(blockId);

        })
    }),
    $(".remove-product-btn").click(function (event) {
        event.preventDefault();
        var blockId = $(this).data("item-id");
        deleteRow(blockId);


    }),
        $('#example').DataTable({
            ordering: true, // Disable sorting for all columns
            paging: false, // Disable paging
            searching: false,
            order: [[0, 'asc']],
        });
    
    
});



function deleteRow(blockId) {
    var blockNum = parseInt(blockId.split('_')[1]);
    tableIndex--;
    $('#example').dataTable().fnDeleteRow(blockNum);
}















function InitCompany() {
    $.ajax({
        url: "/Accountant/Companies/InitCompanyPartial",
        method: "GET",
        success: function (data) {
            if (data != null) {
                $("#CreateCompany .modal-body").html(data);
                $("#CreateCompany").modal("show");
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function GenerateItemBlock() {
    tableIndex++;
    ItemNumber++;
        // Create unique IDs for each element using ItemNumber
    var productID = "Product_" + ItemNumber;
    var unitID = "Unit_" + ItemNumber;
    var unitPriceID = "UnitPrice_" + ItemNumber;
    var quantityID = "Quantity_" + ItemNumber;
    var totalID = "Total_" + ItemNumber;

    $('#example').dataTable().fnAddData([
        ItemNumber-1,
        `<td class="editable">
                                 <input class="form-control typeahead" id="${productID}" name="${productID}" aria-required="true" value=""  required/>
                                <div class="invalid-feedback">Please choose a Product or add new.</div>
                            </td>`,
        `<input class="form-control" id="${unitID}" name="${unitID}" aria-required="true" value="" required/>`,
        ` <input class="form-control" id="${unitPriceID}" name="${unitPriceID}" aria-required="true" value="" required/>`,
        ` <input class="form-control" id="${quantityID}" name="${quantityID}" aria-required="true" value="" required/>`,
        ` <input class="form-control" id="${totalID}" name="${totalID}" aria-required="true" value="" required/>`,
        `<button class="btn btn-primary add-new-product-btn col-md-2" id="@product.Name" data-item-id="@product.Name" style="margin-right:20px; width:fit-content;"> 
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-plus-circle" viewBox="0 0 16 16">
                <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"/>
            </svg>
           
        </button>
        <button class="btn btn-danger remove-product-btn col-md-2" id="@product.Name" data-item-id="Block_${tableIndex}" style="width:fit-content"> 
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5Zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5Zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6Z"/>
                <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1ZM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118ZM2.5 3h11V2h-11v1Z"/>
            </svg>
        </button>`
    ]);
    var table = $('#example').DataTable();

    // Get the row you want to assign an ID to
    var rowIndex = tableIndex; // Index of the row you want to assign an ID to
    var rowNode = table.row(rowIndex).node();

    var rowId = 'Block_' + (tableIndex-1); // ID value you want to assign
    $(rowNode).addClass(rowId);
    var company = document.getElementById("Company_Name").value;
    GetCommonlyUsedProducts(company).then(function (data) {

        CommonProducts = data.data;
    }).catch(function (error) {
        toastr["erro"](error);
    });
     
   
    
}
var CommonProducts;

function GetCommonlyUsedProducts(company) {

    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Accountant/Products/getCommonProducts?company=" + company,
            method: "GET",
            success: function (data) {
                resolve(data);
            },
            error: function () {
                reject("something went wrong");
            }
        });
    });
}


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
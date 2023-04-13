
// File Upload
// 
function ekUpload() {
    function Init() {

        console.log("Upload Initialised");

        var fileSelect = document.getElementById('file-upload'),
            fileDrag = document.getElementById('file-drag'),
            submitButton = document.getElementById('submit-button');

        fileSelect.addEventListener('change', fileSelectHandler, false);

        // Is XHR2 available?
        var xhr = new XMLHttpRequest();
        if (xhr.upload) {
            // File Drop
            fileDrag.addEventListener('dragover', fileDragHover, false);
            fileDrag.addEventListener('dragleave', fileDragHover, false);
            fileDrag.addEventListener('drop', fileSelectHandler, false);
        }
    }

    function fileDragHover(e) {
        var fileDrag = document.getElementById('file-drag');

        e.stopPropagation();
        e.preventDefault();

        fileDrag.className = (e.type === 'dragover' ? 'hover' : 'modal-body file-upload');
    }

    function fileSelectHandler(e) {
        // Fetch FileList object
        var files = e.target.files || e.dataTransfer.files;

        // Cancel event and hover styling
        fileDragHover(e);

        // Process all File objects
        for (var i = 0, f; f = files[i]; i++) {
            parseFile(f);
        }
    }

    // Output
    function output(msg) {
        // Response
        var m = document.getElementById('messages');
        m.innerHTML = msg;
    }

    function parseFile(file) {

        console.log(file.name);
        output(
            '<strong>' + file.name + '</strong>'
        );

        // var fileType = file.type;
        // console.log(fileType);
        var imageName = file.name;

        var isGood = validateFile(file);
        if (isGood) {
            document.getElementById('start').classList.add("hidden");
            document.getElementById('response').classList.remove("hidden");
            document.getElementById('notimage').classList.add("hidden");
            document.getElementById('btn_submit').classList.remove("hidden");


            // Thumbnail Preview
            document.getElementById('file-image').classList.remove("hidden");
            document.getElementById('file-image').src = URL.createObjectURL(file);
        }
        else {
            document.getElementById('file-image').classList.add("hidden");
            document.getElementById('notimage').classList.remove("hidden");
            document.getElementById('start').classList.remove("hidden");
            document.getElementById('response').classList.add("hidden");
            document.getElementById("file-upload-form").reset();
        }
    }

    
    

        


    // Check for the various File API support.
    if (window.File && window.FileList && window.FileReader) {
        Init();
    } else {
        document.getElementById('file-drag').style.display = 'none';
    }
}
ekUpload();


/** Data tables*/
$(document).ready(function () {
    $("#myTable").DataTable();
})

function validateFile(file) {
    const allowedExtensions = ["pdf", "jpg", "jpeg", "png"];
    const maxFileSize = 5 * 1024 * 1024; // 5MB
    const allowedMimeTypes = ["application/pdf", "image/jpeg", "image/png"];

    // Check file extension
    const fileExtension = file.name.split(".").pop().toLowerCase();
    if (!allowedExtensions.includes(fileExtension)) {
        alert("Invalid file extension. Allowed extensions are: " + allowedExtensions.join(", "));
        return false;
    }

    // Check file size
    if (file.size > maxFileSize) {
        alert("File size exceeds the limit. Maximum file size is " + (maxFileSize / (1024 * 1024)) + "MB");
        return false;
    }

    // Check MIME type
    if (!allowedMimeTypes.includes(file.type)) {
        alert("Invalid file type. Allowed file types are: " + allowedMimeTypes.join(", "));
        return false;
    }

    return true;
}
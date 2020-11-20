
$(function () {
    //initialize Croppie
    //var basic = $('#main-cropper').croppie
    //    ({
    //        viewport: { width: 200, height: 200 },
    //        boundary: { width: 400, height: 400 },            
    //        showZoomer: true,
    //        //url: '/DefaultImages/preview.jpg',
    //        format: 'png' //'jpeg'|'png'|'webp'
    //    });  

    var basic = $('#main-cropper').croppie
        ({
            viewport: { width: 150, height: 150 },
            boundary: { width: 300, height: 300 },
            showZoomer: true,
           // enableResize: true,
            enableOrientation: true,
            mouseWheelZoom: 'ctrl'
        });


    //Reading the contents of the specified Blob or File
    function readFile(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#main-cropper').croppie('bind', {
                    url: e.target.result
                });
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    // Change Event to Read file content from File input
    $('#select').on('change', function () { readFile(this); });


    // Upload button to Post Cropped Image to Store.
    $('#btnupload').on('click', function () {
        basic.croppie('result', 'blob').then(function (blob) {
            var formData = new FormData();

            var filename = $('#select').val();
            formData.append('filename', filename);
            formData.append('blob', blob);

            var request = new XMLHttpRequest();
            request.open('POST','/Productos/CustomCrop');
            request.send(formData);
            request.onreadystatechange = function () { // Call a function when the state changes.
                if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
                    var response = JSON.parse(request.responseText);

                    console.log(response.message);                   

                    if (response.message !== "ERROR") {
                        $('#imgProducto').attr('src', 'data:image/png;base64,' + response.message);
                        $('#ModalUploadImagen').modal('hide');
                    }
                }
            }
        });
    });
});
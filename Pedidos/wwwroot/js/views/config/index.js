
$(function () {
    hideLoading();
});

var loading;

function showLoading() {
    loading = Swal.fire({
        showConfirmButton: false,
        allowOutsideClick: false,
        imageAlt: 'A tall image',
        html: "<div class='d-block justify-content-center'> <div class='spinner-border text-primary mr-3' role='status'></div > <div>Espere un momento por favor. </div> </div >"
    })
}

function hideLoading() {
    loading.close();
}
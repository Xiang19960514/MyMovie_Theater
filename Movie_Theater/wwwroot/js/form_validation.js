// 表單驗證
function formValidation() {
    const form = document.querySelector('.needs-validation');
    form.addEventListener('submit', event => {
        //alert('111');
        if (!form.checkValidity()) {
            event.preventDefault()
            event.stopPropagation()
        }
        form.classList.add('was-validated')
    });
}


let buttons = document.querySelectorAll(".deletebutton")

buttons.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    let url = btn.getAttribute("href");

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url).then(response => {
                if (response.status == 200) {
                    window.location.reload(true);
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong!',
                        footer: '<a href="">Why do I have this issue?</a>'
                    })
                }
            })
        }
    })
}));








let xbuttons = document.querySelectorAll(".x-button");

xbuttons.forEach(xbtn => xbtn.addEventListener("click", function () {

    xbtn.parentElement.remove();

}))







function delay(time) {
    return new Promise(resolve => setTimeout(resolve, time));
}

let addbasketbtn = document.querySelectorAll(".add-tobasket");
addbasketbtn.forEach(btn => btn.addEventListener("click", function (e) {

    e.preventDefault()
    let url = btn.getAttribute("href");

    fetch(url).then(response => {

        if (response.status == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Your book has been added',
                showConfirmButton: false,
                timer: 1500
            })
           
            delay(1600).then(() => window.location.reload(true));
                
            
        }
        else {
                window.location.reload(true)
                alert("Book not found!")
            }


       
    }
    )
}))







let delbtn = document.querySelectorAll(".delete-item")

delbtn.forEach(btn => btn.addEventListener("click", function () {
    btn.parentElement.parentElement.remove()

}))

document.addEventListener("DOMContentLoaded", function () {
    var currentUrl = window.location.pathname;

    var menuItems = document.querySelectorAll('.nav-link');

    menuItems.forEach(function (menuItem) {
        menuItem.parentElement.classList.remove('active');
    });


    let check = true;

    menuItems.forEach(function (menuItem, index) {
        var href = menuItem.getAttribute('href');


        if (currentUrl.includes(href) && index > 0) {

            console.log(index + "  index");
            console.log(currentUrl + "  url");


            console.log("bax");
            menuItem.parentElement.classList.add('active');
            check = false;
        }
    });

    if (check) {
        menuItems[0].parentElement.classList.add("active");
    }
});


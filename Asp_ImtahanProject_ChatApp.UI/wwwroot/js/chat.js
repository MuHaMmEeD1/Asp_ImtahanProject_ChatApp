
var connection = new signalR.HubConnectionBuilder().withUrl("/friendHub")
    .build();


connection.on("OnConnectedMethod", (name, imageUrl) => {
  
    const userName = document.getElementById("userName");
    const userProfileImage = document.getElementById("userProfileImage");

    userName.innerHTML = name;

    if (imageUrl) {

        userProfileImage.src = imageUrl;
    }
    else {
        userProfileImage.src = "/assets/images/defaultProfileImage.png";
    }

});

async function Start() {
    try {
        await connection.start(); 

    } catch (err) {
        console.error(err);
    }
}

Start();

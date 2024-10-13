


// Hub Start

var connection = new signalR.HubConnectionBuilder().withUrl("/friendHub")
    .build();


connection.on("OnConnectedMethod", (name, imageUrl, fullName, Email, id) => {
  
    const myUserId = document.getElementById("myUserId");
    const userName = document.getElementById("userName");
    const userProfileImage = document.getElementById("userProfileImage");
    const userFullName = document.getElementById("userFullName");
    const userEmail = document.getElementById("userEmail");


    myUserId.innerHTML = id;

    userName.innerHTML = name;
    userProfileImage.src = imageUrl;
    userFullName.innerHTML = fullName;

    userEmail.innerHTML = Email;

    var currentUrl = window.location.pathname;

    if (currentUrl.toLowerCase() == "/home/notifications") {
        showNotificationsFunction();
    }
    else if (currentUrl.toLowerCase() == "/home/index") {
        defaultFriendPostsMethod();
    }
    else if (currentUrl.toLowerCase() == "/home/friends") {
        showFriendsMethod();
    }
    else if (currentUrl.toLowerCase() == "/home/messages") {
        showUserFriendMessage();
    }

    showHeaderUnansweredFriendshipRequest();

    
});

connection.on("HeaderReflash", (userId) => {

   

    const myUserId = document.getElementById("myUserId");
    var currentUrl = window.location.pathname;


   

    if (myUserId.innerHTML == userId) {


        showHeaderUnansweredFriendshipRequest();
        showHeaderFriendshipRequest();


        if (currentUrl == "/Home/Notifications") {
            showNotificationsFunction();
        }
    }

});

connection.on("PostUlReflashStart", (tagName) => {


    const searchInputValue = document.getElementById("searchInputValue");

    if (searchInputValue.innerHTML == tagName) {
        searchPostIsTagMethod();
    }
    else if (searchInputValue.innerHTML == "") {
        defaultFriendPostsMethod();

    }

});

connection.on("PostUlReflash_ID_Start", (userId) => {

    const searchInputValue = document.getElementById("searchInputValue");
    const myUserId = document.getElementById("myUserId");

    var currentUrl = window.location.pathname;


    if (currentUrl.toLocaleLowerCase() == "/home/index" && myUserId.innerHTML == userId) {

        if (searchInputValue == "") {
            defaultFriendPostsMethod();

        }
        else {
            searchPostIsTagMethod();

        }
    }

});

connection.on("FriendsReflash", (userId) => {
    var currentUrl = window.location.pathname;
    const myUserId = document.getElementById("myUserId");


    if (myUserId.innerHTML == userId && currentUrl.toLowerCase() == "/home/friends") {
        showFriendsMethod();
    }

});

connection.on("MessageReflash", (userId, otherUserId, otherProfileUrl, otherUserName) => {

    var currentUrl = window.location.pathname;
    const myUserId = document.getElementById("myUserId");

    if (currentUrl.toLowerCase() == "/home/messages") {

        if (myUserId.innerHTML == userId) {

            showMessageFriendAndUser(otherUserId, otherProfileUrl, otherUserName);
        }
    }

})

async function InvokePostUlReflash() {
    const searchInput = document.getElementById("searchInput");
    await connection.invoke("PostUlReflash", searchInput.value);
    console.log("Invoke Ok " + searchInput.value);
}

async function InvokeHeaderReflash(userId) {
    await connection.invoke("HeaderReflash", userId)
}

async function InvokePostUlReflash_ID_Start(userId) {
    await connection.invoke("PostUlReflash_ID", userId);
}

async function InvokeFriendsReflashStart(userId) {
    await connection.invoke("FriendsReflash", userId);
}

async function InvokeMessageReflashStart(userId, otherUserId, otherProfileUrl, otherUserName) {
    await connection.invoke("MessageReflash", userId, otherUserId, otherProfileUrl, otherUserName);
}
// Hub End







// Addition Functions Start
function updateLabel(input) {
    var label = document.querySelector('label[for="Photo"]');
    if (input.files && input.files.length > 0) {
        label.textContent = input.files[0].name;
    } else {
        label.textContent = "Image Not Selected";
    }
}



// Addition Functions Start


// HomeIndex Start
function searchPostIsTag(event) {
    event.preventDefault();

    const searchInputValue = document.getElementById("searchInputValue");
    const searchInput = document.getElementById("searchInput");

    searchInputValue.innerHTML = searchInput.value;

    var currentUrl = window.location.pathname;

    if (currentUrl.toLocaleLowerCase() == "/home/index") {

        searchPostIsTagMethod();
    }
  
}

//FriendsPosts

function searchPostIsTagMethod() {

    const searchInput = document.getElementById("searchInput");
    const postsUl = document.getElementById("postsUl");
    const userProfileImage = document.getElementById("userProfileImage");
    const myUserId = document.getElementById("myUserId");

    let replyed = false;

    if (searchInput.value.trim() === "") {
        defaultFriendPostsMethod();
        console.log("exit");
        return;
    }


    $.ajax({
        url: `/Post/GetSearchPost/${encodeURIComponent(searchInput.value)}`,
        type: 'GET',
        contentType: 'application/json',
        success: function (posts) {
            console.dir(posts.$values);

            if (posts.$values.length === 0) {
                console.log("0 element");
                postsUl.innerHTML = '<li>No posts found.</li>';
                return;
            } else {
                postsUl.innerHTML = '';

                posts.$values.forEach((post, index) => {
                    const listItem = document.createElement('li');
                    let itIsMyPost = myUserId.innerHTML == post.userId;

                    let userLikeId = -1;

                    post.likes.$values.forEach((like) => {

                        if (like.userId == myUserId.innerHTML) {
                            userLikeId = like.id;
                        }


                    });


                    listItem.innerHTML = `
                                <div class="news-feed news-feed-post" style="margin-bottom: 30px;">
                                    <div class="post-header d-flex justify-content-between align-items-center">
                                        <div class="image">
                                            <a href="my-profile.html"><img src="${post.userProfileImageUrl}" class="rounded-circle" alt="image"></a>
                                        </div>
                                        <div class="info ms-3">
                                            <span class="name"><a href="my-profile.html">${post.userName}</a></span>
                                            <span class="small-text"><a href="#">${post.dateTime}</a></span>
                                        </div>
                                        <div class="dropdown">
                                            ${itIsMyPost ? '' : `
                                            <button class="dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                <i class="flaticon-menu"></i>
                                            </button>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <a class="dropdown-item btn btn-success d-flex align-items-center" onclick="addFriendshipRequest(event, '${post.userId}', ${index})">
                                                        <i class="flaticon-edit"></i>
                                                        <span id="addFriendP${index}" style="margin-left: 8px;">Add Friend</span>
                                                        <p style="display: none" id = "addFriendPNumber${index}" ></p>
                                                    </a>
                                                </li>
                                            </ul>`}
                                        </div>
                                    </div>
                                    <div class="post-body">
                                        <p>${post.text}</p>
                                        <div class="post-image">
                                            ${post.imageUrl ? `<img src="${post.imageUrl}" alt="image" style="width: 100%; height: auto; object-fit: contain;">` : ''}
                                        </div>
                                        ${post.videoLink ? `
                                        <div class="embed-responsive" style="width: 100%; max-width: 1000px; height: 400px; margin: 0 auto;">
                                            <iframe class="embed-responsive-item" 
                                                style="width: 100%; height: 100%;"
                                                src="${post.videoLink.includes('youtu.be') ? post.videoLink.replace('youtu.be/', 'youtube.com/embed/') : post.videoLink.replace('watch?v=', 'embed/')}" 
                                                frameborder="0" allowfullscreen>
                                            </iframe>
                                        </div>` : ''}
                                        <ul class="post-meta-wrap d-flex justify-content-between align-items-center">
                                            <li class="post-react">
                                                <a id="likePostButton${index}" 
                                                   class="btn d-flex align-items-center border-0 text-decoration-none"
                                                   onclick="likePostFunction(event, ${index}, ${post.postId}, ${userLikeId})">
                                                    <i id="likePostStyle${index}" 
                                                       class="fas fa-thumbs-up fa-3x" 
                                                       style="color: ${userLikeId > -1 ? 'blue' : 'darkgray'}; font-size: 30px;" 
                                                       title="${userLikeId > -1 ? 'Liked' : 'Like'}"></i>
                                                    <span class="number ml-2">${post.likes.$values.length}</span>
                                                </a>
                                            </li>
                                            <li class="post-comment" onclick="openCloseComments(event, ${index})">
                                                <a><i class="flaticon-comment"></i>
                                                <span>Comment</span> <span class="number">${post.comments.$values.length}</span>
                                                </a>
                                            </li>
                                        </ul>
                                        <div id="commentDiv${index}" class="post-comment-list" style="display: block;"></div>
                                        <form class="post-footer">
                                            <div class="footer-image">
                                                <a href="#"><img src="${userProfileImage.src}" class="rounded-circle" alt="image"></a>
                                            </div>
                                            <div class="form-group d-flex align-items-center">
                                                <p id="replyCommetnId${index}" style="display: none;"></p>
                                                <textarea id="addCommentInput${index}" name="message" class="form-control me-2" placeholder="Write a comment..."></textarea>
                                                <button type="button" class="btn btn-primary" onclick="addCommentFunction(event, ${index}, ${post.postId})">Add Comment</button>
                                                <button id="declineReplyButton${index}" style="display: none;" type="button" class="btn btn-primary ms-2" onclick="declineReplyFunction(event, ${index})">Decline Reply</button>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            `;


                  


                    postsUl.appendChild(listItem);



                    const commentDiv = document.getElementById(`commentDiv${index}`);
                    commentDiv.innerHTML = "";

                    if (!itIsMyPost) {

                        myFriendCheckFunction(index, post.userId);
                    }
                   

                    post.comments.$values.forEach((comment, commentIndex) => {

                        console.log(commentIndex+" Comment");

                        commentDiv.innerHTML +=  `
                            <div class="comment-list">
                                <div class="comment-image">
                                    <a ><img src="${comment.userProfileImageUrl}" class="rounded-circle" alt="image"></a>
                                </div>
                                <div class="comment-info">
                                    <h3>
                                        <h4>${comment.userName}</h4>
                                    </h3>
                                    <span>${comment.dateTime}</span>
                                    <p>${comment.text}</p>

                                    <ul class="comment-react">
                                        <li><a onclick="replyButtonFunction( event , ${index} , '${comment.id}', '${comment.userName}' )">Reply</a></li> 
                                    </ul>
                                </div>
                            </div>

                            <div id="replyToCommentDiv${commentIndex}" style="display: block;"   class="more-comments"></div>

                            <div class="more-comments" style="margin-bottom: 20px;">
                              <a onclick='openCloseReplyToComment(event, ${commentIndex})'>More Comments+</a>

                            </div> 
                        `;


                        const replyToCommentDiv = document.getElementById(`replyToCommentDiv${commentIndex}`);
                        replyToCommentDiv.innerHTML = "";

                        comment.replyToComments.$values.forEach((rtc) => {


                            replyToCommentDiv.innerHTML += `
                             <div class="comment-list" style="display: flex; align-items: center; justify-content: space-between; font-size: 12px; padding-top: 30px; position: relative; left: 50px;">

                               
                                 <div class="comment-info" style="flex-grow: 1; text-align: left;">
                                     <h4 style="margin: 0; font-size: 16px;">${rtc.userName}</h4>
                                     <span style="font-size: 13px; color: #777;">${rtc.dateTime}</span>
                                     <p style="font-size: 14px;">${rtc.text}</p>
                                 </div>
     
                                 <div class="comment-image" style="margin-left: 10px; margin-top: 30px">
                                     <a><img src="${rtc.userProfileImageUrl}" class="rounded-circle" alt="image" style="width: 50px; height: 50px;"></a>
                                 </div>
                             </div>
                            `;




                        });
                       
                    })
                });
            }
        },
        error: function (xhr, status, error) {
            console.log('alax errore');
            console.error("Error fetching posts:", status, error);
        }
    });
}

function defaultFriendPostsMethod() {

    const postsUl = document.getElementById("postsUl");
    const userProfileImage = document.getElementById("userProfileImage");
    const myUserId = document.getElementById("myUserId");


    $.ajax({
        url: `/Post/FriendsPosts/${myUserId.innerHTML}`,
        type: 'GET',
        contentType: 'application/json',

        success: function (posts) {
            console.dir(posts.$values);
            console.log("Friend Post OK");
           
            postsUl.innerHTML = '';

            posts.$values.forEach((post, index) => {
                const listItem = document.createElement('li');
                let itIsMyPost = myUserId.innerHTML == post.userId;

                let userLikeId = -1;

                post.likes.$values.forEach((like) => {

                    if (like.userId == myUserId.innerHTML) {
                        userLikeId = like.id;
                    }


                });


                listItem.innerHTML = `
                            <div class="news-feed news-feed-post" style="margin-bottom: 30px;">
                                <div class="post-header d-flex justify-content-between align-items-center">
                                    <div class="image">
                                        <a href="my-profile.html"><img src="${post.userProfileImageUrl}" class="rounded-circle" alt="image"></a>
                                    </div>
                                    <div class="info ms-3">
                                        <span class="name"><a href="my-profile.html">${post.userName}</a></span>
                                        <span class="small-text"><a href="#">${post.dateTime}</a></span>
                                    </div>
                                    <div class="dropdown">
                                        ${itIsMyPost ? '' : `
                                        <button class="dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            <i class="flaticon-menu"></i>
                                        </button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <a class="dropdown-item btn btn-success d-flex align-items-center" onclick="addFriendshipRequest(event, '${post.userId}', ${index})">
                                                    <i class="flaticon-edit"></i>
                                                    <span id="addFriendP${index}" style="margin-left: 8px;">Add Friend</span>
                                                    <p style="display: none" id = "addFriendPNumber${index}" ></p>
                                                </a>
                                            </li>
                                        </ul>`}
                                    </div>
                                </div>
                                <div class="post-body">
                                    <p>${post.text}</p>
                                    <div class="post-image">
                                        ${post.imageUrl ? `<img src="${post.imageUrl}" alt="image" style="width: 100%; height: auto; object-fit: contain;">` : ''}
                                    </div>
                                    ${post.videoLink ? `
                                    <div class="embed-responsive" style="width: 100%; max-width: 1000px; height: 400px; margin: 0 auto;">
                                        <iframe class="embed-responsive-item" 
                                            style="width: 100%; height: 100%;"
                                            src="${post.videoLink.includes('youtu.be') ? post.videoLink.replace('youtu.be/', 'youtube.com/embed/') : post.videoLink.replace('watch?v=', 'embed/')}" 
                                            frameborder="0" allowfullscreen>
                                        </iframe>
                                    </div>` : ''}
                                    <ul class="post-meta-wrap d-flex justify-content-between align-items-center">
                                        <li class="post-react">
                                            <a id="likePostButton${index}" 
                                                class="btn d-flex align-items-center border-0 text-decoration-none"
                                                onclick="likePostFunction(event, ${index}, ${post.postId}, ${userLikeId})">
                                                <i id="likePostStyle${index}" 
                                                    class="fas fa-thumbs-up fa-3x" 
                                                    style="color: ${userLikeId > -1 ? 'blue' : 'darkgray'}; font-size: 30px;" 
                                                    title="${userLikeId > -1 ? 'Liked' : 'Like'}"></i>
                                                <span class="number ml-2">${post.likes.$values.length}</span>
                                            </a>
                                        </li>
                                        <li class="post-comment" onclick="openCloseComments(event, ${index})">
                                            <a><i class="flaticon-comment"></i>
                                            <span>Comment</span> <span class="number">${post.comments.$values.length}</span>
                                            </a>
                                        </li>
                                    </ul>
                                    <div id="commentDiv${index}" class="post-comment-list" style="display: block;"></div>
                                    <form class="post-footer">
                                        <div class="footer-image">
                                            <a href="#"><img src="${userProfileImage.src}" class="rounded-circle" alt="image"></a>
                                        </div>
                                        <div class="form-group d-flex align-items-center">
                                            <p id="replyCommetnId${index}" style="display: none;"></p>
                                            <textarea id="addCommentInput${index}" name="message" class="form-control me-2" placeholder="Write a comment..."></textarea>
                                            <button type="button" class="btn btn-primary" onclick="addCommentFunction(event, ${index}, ${post.postId})">Add Comment</button>
                                            <button id="declineReplyButton${index}" style="display: none;" type="button" class="btn btn-primary ms-2" onclick="declineReplyFunction(event, ${index})">Decline Reply</button>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        `;





                postsUl.appendChild(listItem);



                const commentDiv = document.getElementById(`commentDiv${index}`);
                commentDiv.innerHTML = "";

                if (!itIsMyPost) {

                    myFriendCheckFunction(index, post.userId);
                }


                post.comments.$values.forEach((comment, commentIndex) => {

                    console.log(commentIndex + " Comment");

                    commentDiv.innerHTML += `
                        <div class="comment-list">
                            <div class="comment-image">
                                <a ><img src="${comment.userProfileImageUrl}" class="rounded-circle" alt="image"></a>
                            </div>
                            <div class="comment-info">
                                <h3>
                                    <h4>${comment.userName}</h4>
                                </h3>
                                <span>${comment.dateTime}</span>
                                <p>${comment.text}</p>

                                <ul class="comment-react">
                                    <li><a onclick="replyButtonFunction( event , ${index} , '${comment.id}', '${comment.userName}' )">Reply</a></li> 
                                </ul>
                            </div>
                        </div>

                        <div id="replyToCommentDiv${commentIndex}" style="display: block;"   class="more-comments"></div>

                        <div class="more-comments" style="margin-bottom: 20px;">
                            <a onclick='openCloseReplyToComment(event, ${commentIndex})'>More Comments+</a>

                        </div> 
                    `;


                    const replyToCommentDiv = document.getElementById(`replyToCommentDiv${commentIndex}`);
                    replyToCommentDiv.innerHTML = "";

                    comment.replyToComments.$values.forEach((rtc) => {


                        replyToCommentDiv.innerHTML += `
                            <div class="comment-list" style="display: flex; align-items: center; justify-content: space-between; font-size: 12px; padding-top: 30px; position: relative; left: 50px;">

                               
                                <div class="comment-info" style="flex-grow: 1; text-align: left;">
                                    <h4 style="margin: 0; font-size: 16px;">${rtc.userName}</h4>
                                    <span style="font-size: 13px; color: #777;">${rtc.dateTime}</span>
                                    <p style="font-size: 14px;">${rtc.text}</p>
                                </div>
     
                                <div class="comment-image" style="margin-left: 10px; margin-top: 30px">
                                    <a><img src="${rtc.userProfileImageUrl}" class="rounded-circle" alt="image" style="width: 50px; height: 50px;"></a>
                                </div>
                            </div>
                        `;




                    });

                })
            });
            
        },
         error: function (xhr, status, error) {
            console.log('alax errore');
            console.error("Error fetching posts:", status, error);
        }


    });
}


function likePostFunction(event, index, postId, userLikeId) {
    event.preventDefault();

    const likePostStyle = document.getElementById(`likePostStyle${index}`);
    const myUserId = document.getElementById("myUserId");

    console.log(postId +" P id");
    console.log(myUserId.innerHTML + " U id");
    console.log(userLikeId);


    if (likePostStyle.style.color === 'darkgray' || likePostStyle.style.color === '') {
        likePostStyle.style.color = 'blue';

        $.ajax({
            url: '/Like/Add',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                PostId: postId,
                UserId: myUserId.innerHTML
            }),
            success: function () {
                console.log("Like added successfully");
                InvokePostUlReflash();
            },
            error: function (xhr, status, error) {
                console.error("Error: " + error);
                console.error("Response: " + xhr.responseText);
            }
        });

    } else {
        likePostStyle.style.color = 'darkgray';

        $.ajax({
            url: '/Like/Delete',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                Id: userLikeId
            }),
            success: function () {
                InvokePostUlReflash();
            },
            error: function () { }
        });
    }
}



function declineReplyFunction(event, index) {

    event.preventDefault();

    const addCommentInput = document.getElementById(`addCommentInput${index}`)
    const replyCommetnId = document.getElementById(`replyCommetnId${index}`);
    const declineReplyButton = document.getElementById(`declineReplyButton${index}`);

    declineReplyButton.style.display = "none";   
    replyCommetnId.innerHTML = "";
    addCommentInput.placeholder = "Write a comment...";

}

function openCloseReplyToComment(event, index) {
    event.preventDefault();

    console.log("OC_RTC");
    const replyToCommentDiv = document.getElementById(`replyToCommentDiv${index}`);

    
    if (replyToCommentDiv.style.display === "none" || replyToCommentDiv.style.display === "") {
        replyToCommentDiv.style.display = "block"; 
        console.log("ReplyToComment Open");

    }
    else {
        replyToCommentDiv.style.display = "none";   
        console.log("ReplyToComment Close");

    }
}


function replyButtonFunction(event, index, commentId, userName) {

    event.preventDefault();

    const replyCommentId = document.getElementById(`replyCommetnId${index}`);
    const addCommentInput = document.getElementById(`addCommentInput${index}`)
    const declineReplyButton = document.getElementById(`declineReplyButton${index}`);


    addCommentInput.focus();

    addCommentInput.placeholder = `reply ${userName}:`;
    replyCommentId.innerHTML = commentId;
    declineReplyButton.style.display = "block";   

    console.log("reply ok");

}


function openCloseComments(event, index) {
    event.preventDefault(); 

    const commentDiv = document.getElementById(`commentDiv${index}`);

   
    if (commentDiv.style.display === "none" || commentDiv.style.display === "") {

        commentDiv.style.display = "block";  
        console.log("Comment Open");
    } else {
        commentDiv.style.display = "none";  
        console.log("Comment Close");

    }

}

function addCommentFunction(event, index, postId) {

    event.preventDefault();

    const addCommentInput = document.getElementById(`addCommentInput${index}`);
    const replyCommetnId = document.getElementById(`replyCommetnId${index}`);
    const declineReplyButton = document.getElementById(`declineReplyButton${index}`);

    const myUserId = document.getElementById("myUserId");


    if (addCommentInput.value.trim() !== "") {

        if (replyCommetnId.innerHTML.trim() !== "") {

            $.ajax({
                url: 'ReplyToComment/Add',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    Text: addCommentInput.value,
                    UserId: myUserId.innerHTML,
                    CommentId: replyCommetnId.innerHTML,
                }),
                success: function (response) {
                    console.log("Comment added successfully");
                    addCommentInput.value = '';
                    addCommentInput.placeholder = 'Write a comment...'

                    replyCommetnId.innerHTML = '';
                    declineReplyButton.style.display = "none";  


                    InvokePostUlReflash();

                },
                error: function (xhr, status, error) {
                    console.error("Error adding comment:", status, error);
                }

            });


        }
        else {

            $.ajax({
                url: '/Comment/AddComment',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    Text: addCommentInput.value,
                    PostId: postId,
                    UserId: myUserId.innerHTML
                }),
                success: function (response) {
                    console.log("Comment added successfully");
                    addCommentInput.value = '';

               
                    InvokePostUlReflash();

                },
                error: function (xhr, status, error) {
                    console.error("Error adding comment:", status, error);
                }
            });
        }

    } else {
        alert("Comment cannot be empty!");
    }
}


function createPostFunction(event) {


    event.preventDefault();

    var formData = new FormData($('#createPostForm')[0]);
    const ImageUrlLabel = document.getElementById("ImageUrlLabel");


    $.ajax({
        url: '/Home/CreatePost',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                $('#createPostForm')[0].reset();
                ImageUrlLabel.textContent = "Image Not Selected";

            }
        },
        error: function (xhr, status, error) {
            console.log('An error occurred: ' + error);
        }
    });
}

// HomeIndex End




// Frientship Request Start

function addFriendshipRequest(event, otherUserId, index) {
    event.preventDefault();
    const addFriendPNumber = document.getElementById(`addFriendPNumber${index}`);
    const myUserId = document.getElementById("myUserId");


    if (addFriendPNumber.innerHTML == "2") {
        const model = {
            UserId: myUserId.innerHTML,  
            OtherUserId: otherUserId      
        };

        $.ajax({
            url: '/UserFriend/DeleteUsOuId',  
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),  
            success: function (response) {
                console.log("UserFriend deleted successfully:", response);

                InvokePostUlReflash();
                InvokePostUlReflash_ID_Start(otherUserId);
                InvokeFriendsReflashStart(outherUserId);

            },
            error: function (xhr, status, error) {
                console.error("Error deleting friend:", error);
            }
        });
    }


    else if (addFriendPNumber.innerHTML == "1") {

        const model = {
            UserId: myUserId.innerHTML, 
            OtherUserId: otherUserId 
        };

        $.ajax({
            url: '/FriendshipRequest/DeleteUsOuId',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),
            success: function (response) {
                console.log("Delete ", response);

                
                InvokeHeaderReflash(otherUserId);
                InvokePostUlReflash_ID_Start(otherUserId);

                InvokePostUlReflash();
            },
            error: function (xhr, status, error) {
                console.error("No Delete", error);
            }
        });

    }
    else if (addFriendPNumber.innerHTML == "0") {

        addFriendshipRequestFunction(otherUserId, null);
       

       

    }
    else { console.error("addFriendshipRequest 2,1,0 no found"); }


    InvokeHeaderReflash(otherUserId);
    
}
function addFriendshipRequestFunction(outherUserId, response) {
    const myUserId = document.getElementById("myUserId").innerHTML;

    const data = {
        UserId: myUserId,
        OtherUserId: outherUserId,
        Response: response,
        }

    

    $.ajax({
        url: '/FriendshipRequest/Add',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (response) {
            console.log("Friendship request sent successfully!");
            InvokeHeaderReflash(outherUserId);
            InvokePostUlReflash();
        },
        error: function (error) {
            console.log("Error while sending friendship request", error);
        }
    });
}


function showHeaderFriendshipRequest() {
   

    $.ajax({
        url: '/FriendshipRequest/GetListDidItAppear',
        type: 'GET',
        success: function (response) {
            console.dir(response.$values);
            const friendshipRequestListDiv = document.getElementById("friendshipRequestListDiv");
            const friendshipRequestSpanCount = document.getElementById("friendshipRequestSpanCount");

            friendshipRequestListDiv.innerHTML = "";
            console.log("showHeaderFriendshipRequest -> "+response.$values);
            if (response.$values.length > 0) {

                console.log("showHeaderFriendshipRequest work");

                friendshipRequestSpanCount.style.display = "block";
                friendshipRequestSpanCount.innerHTML = response.$values.length;

                response.$values.forEach((itemFriendshep, index) => {


                    friendshipRequestListDiv.innerHTML += `
            
                         <div class="item d-flex justify-content-between align-items-center">
                            <div class="figure">
                                <a ><img src="${itemFriendshep.outherUserProfileImageUrl}" class="rounded-circle" alt="image"></a>
                            </div>
                            <div class="text">
                                <h4><a >${itemFriendshep.outherUserName}</a></h4>
                                <span>
                                ${itemFriendshep.response == null ? `wants to be friends with you`
                            : itemFriendshep.response == true ? `accepted your friendship offer`
                                : `rejected your friend offer`

                        }
                                </span>
                                <span class="main-color">${itemFriendshep.dateTime}</span>
                            </div>
                        </div>

                    `;


                });

            }
            else {
                friendshipRequestSpanCount.style.display = "none";
            }

           
        },
        error: function (error) {
            console.log('An error occurred: ' + error);
        }
    });
}
function showHeaderUnansweredFriendshipRequest() {


    $.ajax({
        url: '/FriendshipRequest/GetListUnanswered',
        type: 'GET',
        success: function (response) {
            console.dir(response.$values);
            const replyToFriendshipRequestSapanCount = document.getElementById("replyToFriendshipRequestSapanCount");
            const replyToFriendshipRequestListDiv = document.getElementById("replyToFriendshipRequestListDiv");

            console.log("showHeaderUnansweredFriendshipRequest -> " + response.$values);

            if (response.$values.length > 0) {

                console.log("showHeaderUnansweredFriendshipRequest work");

                replyToFriendshipRequestSapanCount.style.display = "block";
                replyToFriendshipRequestSapanCount.innerHTML = response.$values.length;
                replyToFriendshipRequestListDiv.innerHTML = "";

                response.$values.forEach((itemFriendshep, index) => {


                    replyToFriendshipRequestListDiv.innerHTML += `
            
                         <div class="item d-flex align-items-center">
                            <div class="figure">
                                <a ><img src="${itemFriendshep.outherUserProfileImageUrl}" class="rounded-circle" alt="image"></a>
                            </div>

                            <div class="content d-flex justify-content-between align-items-center">
                                <div class="text">
                                    <h4><a >${itemFriendshep.outherUserName}</a></h4>
                                    <span>
                                        ${itemFriendshep.response == null ? `wants to be friends with you`
                            : itemFriendshep.response == true ? `accepted your friendship offer`
                                : `rejected your friend offer`

                        }
                                </span>
                                <span class="main-color">${itemFriendshep.dateTime}</span>
                                </div>
                                <div class="btn-box d-flex align-items-center">
                                    <button onclick="deleteFriendNotificationsEvent(event, ${index}, '${itemFriendshep.otherUserId}', ${itemFriendshep.id})" class="delete-btn d-inline-block me-2" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete" type="button"><i class="ri-close-line"></i></button>

                                    <button onclick="addFriendNotificationsEvent(event, ${index}, '${itemFriendshep.otherUserId}', ${itemFriendshep.id})" class="confirm-btn d-inline-block" data-bs-toggle="tooltip" data-bs-placement="top" title="Confirm" type="button"><i class="ri-check-line"></i></button>
                                </div>
                            </div>
                        </div>

                    `;


                });

            }
            else {
                replyToFriendshipRequestListDiv.innerHTML = "";
                replyToFriendshipRequestSapanCount.style.display = "none";
            }

        },
        error: function (error) {
            console.log('An error occurred: ' + error);
        }
    });
}

function myFriendCheckFunction(index, outherUserId) {

    const addFriendP = document.getElementById(`addFriendP${index}`);
    const addFriendPNumber = document.getElementById(`addFriendPNumber${index}`);

    if (!addFriendP) {
        return; 
    }

    $.ajax({
        url: '/FriendshipRequest/GetCheckMyFriend', 
        data: { OutherUserId: outherUserId }, 
        success: function (response) {
            console.log(response + " FR End");
            if (response === 2) {
                addFriendP.innerHTML = 'Delete Friend';
                addFriendPNumber.innerHTML = "2";
            } else if (response === 1) {
                addFriendP.innerHTML = 'Delete Friendship Message';
                addFriendPNumber.innerHTML = "1";
            } else {
                addFriendP.innerHTML = 'Add Friend';
                addFriendPNumber.innerHTML = "0";

            }
        },
        error: function (error) {
            console.log('An error occurred: ' + error);
        }
    });

}

function addFriendNotificationsEvent(event, index, otherUserId, frId) {

    const myUserId = document.getElementById("myUserId").innerHTML;

    console.log(myUserId);
    console.log(otherUserId);

    const data = {
        UserFriendFirstId: myUserId,
        UserFriendSecondId: otherUserId
    };

    $.ajax({
        url: '/UserFriend/Add',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (response) {
            console.log("Friend added successfully:", response);

            deleteFriendshipRequestFunction(frId);
            addFriendshipRequestFunction(otherUserId, true)


            InvokeHeaderReflash(myUserId);
            InvokeHeaderReflash(otherUserId);
            InvokePostUlReflash();
            InvokePostUlReflash_ID_Start(otherUserId);


        },
        error: function (xhr, status, error) {

            console.log(myUserId);
            console.log(otherUserId);

            console.error("An error occurred: " + status.toString(), error);
        }
    });
}

function deleteFriendNotificationsEvent(event, index, otherUserId, frId) {

    event.preventDefault();

    console.log("I am Worked deleteFriendNotificationsEvent");
        
    const myUserId = document.getElementById("myUserId").innerHTML;

    deleteFriendshipRequestFunction(frId);
    addFriendshipRequestFunction(otherUserId, false)


    InvokeHeaderReflash(myUserId);
    InvokeHeaderReflash(otherUserId);
    InvokePostUlReflash();
    InvokePostUlReflash_ID_Start(otherUserId);

}


// Frientship Request End



// Notifications Start

function showNotificationsFunction() {




    $.ajax({
        url: '/FriendshipRequest/GetList',
        type: 'GET',
        success: function (response) {
            console.dir(response.$values);
            const notificationsListDiv = document.getElementById("notificationsListDiv");
            notificationsListDiv.innerHTML = "";

            response.$values.forEach((itemFriendshep, index) => {

                notificationsListDiv.innerHTML += `
            
                     <div class="item d-flex justify-content-between align-items-center">
                        <div class="figure">
                            <a ><img src="${itemFriendshep.outherUserProfileImageUrl}" class="rounded-circle" alt="image"></a>
                        </div>
                        <div class="text">
                            <h4><a >${itemFriendshep.outherUserName}</a></h4>
                             <span>
                            ${itemFriendshep.response == null ? `wants to be friends with you`
                        : itemFriendshep.response == true ? `accepted your friendship offer`
                            : `rejected your friend offer`

                    }
                            </span>
                            <span class="main-color">${itemFriendshep.dateTime}</span>
                        </div>
                        <div class="icon">
                            <a onclick="deleteFriendshipRequest(event, ${itemFriendshep.id}, ${itemFriendshep.response}, '${itemFriendshep.otherUserId}')"><i class="flaticon-x-mark"></i></a>
                        </div>
                    </div>

                `;


            });


        },
        error: function (error) {
            console.log('An error occurred: ' + error);
        }
    });

}

function deleteFriendshipRequest(event, friendshipRequestId, response, otherUserId) {
    event.preventDefault();
    const myUserId = document.getElementById("myUserId");



    if (response == null) {
        addFriendshipRequestFunction(otherUserId, false);
    }

    deleteFriendshipRequestFunction(friendshipRequestId);

   
}

function deleteFriendshipRequestFunction(friendshipRequestId) {
    const myUserId = document.getElementById("myUserId");


    $.ajax({
        url: '/FriendshipRequest/Delete',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Id: friendshipRequestId }),
        success: function (response) {
            console.log('Friendship request deleted:', response);

            InvokeHeaderReflash(myUserId.innerHTML);


        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
        }
    });
}


function notificationsClickEvent(event) {

    event.preventDefault();

    const friendshipRequestSpanCount = document.getElementById("friendshipRequestSpanCount");


    $.ajax({
        url: '/FriendshipRequest/GetListDidItAppear',
        type: 'GET',
        success: function (response) {


            
            response.$values.forEach((itemFriendshep, index) => {
                const data = {
                    Id: itemFriendshep.id,
                    DidItAppear: true
                };

                $.ajax({
                    type: "POST",
                    url: "/FriendshipRequest/Update",
                    contentType: "application/json",
                    data: JSON.stringify(data),
                    success: function (response) {
                        console.log("DidItAppear ok Update " + index);
                    },
                    error: function (error) {
                        console.error("Errore:", error);
                    }
                });
            });
           
            
        },
        error: function (error) {
            console.log('Errore: ' + error);
        }
    });

    friendshipRequestSpanCount.style.display = "none";
}


// Notifications End


// DefaultBackgroundProfileImage.png

// Friends Start

function showFriendsMethod() {
    const friendsListDiv = document.getElementById("friendsListDiv");
    const searchFriendInputValue = document.getElementById("searchFriendInputValue");

    if (searchFriendInputValue=="") {

        $.ajax({
            url: `/UserFriend/Get`,
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                friendsListDiv.innerHTML = '';

                console.dir(response.$values);
                response.$values.forEach(function (friend, index) {


                    friendsListDiv.innerHTML +=
                        `<div class="col-lg-4 col-md-6 mb-4"> 
        <div class="card">
            <div class="friends-image" style="background-image: url('${friend.profileBackgroundImageUrl}'); height: 150px; background-size: cover; background-position: center; width: 100%;">
                <!-- Arka plan resmi doğrudan burada ayarlandı -->
            </div>
            <div class="card-body">
                <div class="d-flex align-items-center">
                    <img src="${friend.profileImageUrl}" alt="Profile Image" class="rounded-circle" width="180" height="180" style="margin-right: 20px;"> <!-- Profil fotoğrafı sağa kaydırıldı -->
                    <div class="ms-3">
                        <h5 style="font-size: 2rem; margin-bottom: 0.5rem;">${friend.userName}</h5>
                        <div class="d-flex">
                            <div class="me-3" style="margin-right: 2rem;">
                                <span class="item-number" style="font-size: 1.75rem;">${friend.likeCount}</span>
                                <span class="item-text" style="font-size: 1.25rem;">Likes</span>
                            </div>
                            <div>
                                <span class="item-number" style="font-size: 1.75rem;">${friend.friendCount}</span>
                                <span class="item-text" style="font-size: 1.25rem;">Friends</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="d-flex justify-content-end align-items-center mt-8">
                    ${friend.thisMyFriend ?
                            `<button onclick="deleteFriendInFriends(event, '${friend.outherUserId}')" type="button" class="btn btn-danger">Delete Friend</button>` :
                            `<button type="button" class="btn btn-primary">Add Friend</button>`}
                </div>
            </div>
        </div>
    </div>`;







                    myFriendCheckFriendFunction(index, friend.outherUserId);

                });
            },
            error: function (xhr, status, error) {
                console.error("An error occurred: ", error);
            }
        });
    }
    else {

       


        $.ajax({
            url: `/UserFriend/Get?UserName=${searchFriendInputValue.innerHTML}`,
            type: 'GET', 
            dataType: 'json',
            contentType: 'application/json', 
           
            success: function (response) {
                friendsListDiv.innerHTML = '';

                console.dir(response.$values);
                response.$values.forEach(function (friend, index) {
                    friendsListDiv.innerHTML +=
                        `<div class="col-lg-4 col-md-6 mb-4"> 
                    <div class="card">
                        <div class="friends-image" style="background-image: url('${friend.profileBackgroundImageUrl}'); height: 150px; background-size: cover; background-position: center; width: 100%;"></div>
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <img src="${friend.profileImageUrl}" alt="Profile Image" class="rounded-circle" width="180" height="180" style="margin-right: 20px;">
                                <div class="ms-3">
                                    <h5 style="font-size: 2rem; margin-bottom: 0.5rem;">${friend.userName}</h5>
                                    <div class="d-flex">
                                        <div class="me-3" style="margin-right: 2rem;">
                                            <span class="item-number" style="font-size: 1.75rem;">${friend.likeCount}</span>
                                            <span class="item-text" style="font-size: 1.25rem;">Likes</span>
                                        </div>
                                        <div>
                                            <span class="item-number" style="font-size: 1.75rem;">${friend.friendCount}</span>
                                            <span class="item-text" style="font-size: 1.25rem;">Friends</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="d-flex justify-content-end align-items-center mt-8">
                                ${friend.thisMyFriend ?
                            `<button onclick="deleteFriendInFriends(event, '${friend.outherUserId}')" type="button" class="btn btn-danger">Delete Friend</button>` :
                            `<button type="button" class="btn btn-primary">Add Friend</button>`}
                            </div>
                        </div>
                    </div>
                </div>`;

                    myFriendCheckFriendFunction(index, friend.outherUserId);
                });
            },
            error: function (xhr, status, error) {
                console.error("An error occurred: ", error);
            }
        });

    }
}

function myFriendCheckFriendFunction(index, outherUserId) {

    const friendRankP = document.getElementById(`friendRankP${index}`);
    const friendRankButton = document.getElementById(`friendRankButton${index}`);

    if (!friendRankP) {
        return;
    }

    $.ajax({
        url: '/FriendshipRequest/GetCheckMyFriend',
        data: { OutherUserId: outherUserId },
        success: function (response) {
            if (response === 2) {
                friendRankButton.innerHTML = 'Delete Friend';
                friendRankP.innerHTML = "2";
            } else if (response === 1) {
                friendRankButton.innerHTML = 'Delete Friendship Message';
                friendRankP.innerHTML = "1";
            } else {
                friendRankButton.innerHTML = 'Add Friend';
                friendRankP.innerHTML = "0";

            }
        },
        error: function (error) {
            console.log('An error occurred: ' + error);
        }
    });

}

function deleteFriendInFriends(event, outherUserId) {
    event.preventDefault();

    const myUserId = document.getElementById("myUserId");

    const model = {
        UserId: myUserId.innerHTML,
        OtherUserId: outherUserId
    };

    $.ajax({
        url: '/UserFriend/DeleteUsOuId',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(model),
        success: function (response) {
            console.log("UserFriend deleted successfully:", response);

            InvokePostUlReflash_ID_Start(outherUserId);
            InvokeFriendsReflashStart(outherUserId);
            InvokeFriendsReflashStart(myUserId.innerHTML);

        },
        error: function (xhr, status, error) {
            console.error("Error deleting friend:", error);
        }
    });

}
 
function searchFriendEvent(event) {

    event.preventDefault();
    const searchFriendInputValue = document.getElementById("searchFriendInputValue");
    const searchFriendInput = document.getElementById("searchFriendInput");
    searchFriendInputValue.innerHTML = searchFriendInput.value;

    showFriendsMethod();

}

// Friends End




// Message Start

function showUserFriendMessage() {
    const friendsMessagesList = document.getElementById("friendsMessagesList");
    const friendSearchMseeageInputValue = document.getElementById("friendSearchMseeageInputValue");

    let url = '/UserFriend/FriendsMessages';

    if (friendSearchMseeageInputValue.innerHTML !== "") {
        url += `?UserName=${encodeURIComponent(friendSearchMseeageInputValue.innerHTML)}`;
    }

    $.ajax({
        url: url,
        type: 'GET',
        success: function (data) {
            
            friendsMessagesList.innerHTML = '';

            if (data.$values.length > 0) {

                console.dir(data.$values);

            

                data.$values.forEach((friendMessage, index) => {
                friendsMessagesList.innerHTML += `
                <li onclick="enterFriendMessageChatEvent(event, '${friendMessage.outherUserId}', '${friendMessage.profileImageUrl}', '${friendMessage.userName}')" class="list-group-item d-flex align-items-center">
                    <div class="menu-profile position-relative">
                        <img src="${friendMessage.profileImageUrl}"  class="rounded-circle" style="width: 40px; height: 40px;">
                        <span class="status-online" style="width: 15px; height: 15px; background-color: ${friendMessage.isOnline ? 'green' : 'gray'};"></span>
                    </div>
                    <span class="ml-2">${friendMessage.userName}</span>
                    <span style="color: ${friendMessage.isOnline ? 'green' : 'gray'}" class="badge badge-${friendMessage.isOnline ? 'success' : 'secondary'} badge-pill ml-auto">
                        ${friendMessage.isOnline ? 'Online' : 'Offline'}
                    </span>
                   
                </li>`;
                });

            }
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', status, error);
        }
    });
}

function friendSearchMseeageEvent(event) {

    event.preventDefault();

    const friendSearchMessageInput = document.getElementById("friendSearchMseeageInput");
    const friendSearchMseeageInputValue = document.getElementById("friendSearchMseeageInputValue");

    friendSearchMseeageInputValue.innerHTML = friendSearchMessageInput.value;

    showUserFriendMessage();

}


function enterFriendMessageChatEvent(event, otherUserId, otherProfileUrl, otherUserName) {

    event.preventDefault();
    const otherUserMessageId = document.getElementById("otherUserMessageId");
    const h1OtherUserMessageId = document.getElementById("h1OtherUserMessageId")
    const pOtherUserMessageProfileImage = document.getElementById("pOtherUserMessageProfileImage");
    const pOtherUserMessageUserName = document.getElementById("pOtherUserMessageUserName");

    otherUserMessageId.innerHTML = otherUserId;
    h1OtherUserMessageId.innerHTML = otherUserName;
    pOtherUserMessageProfileImage.src = otherProfileUrl;
    pOtherUserMessageUserName.innerHTML = otherUserName;

    showMessageFriendAndUser(otherUserId, otherProfileUrl, otherUserName);

}

function showMessageFriendAndUser(otherUserId, otherProfileUrl, otherUserName) {
    const userFriendMessageDiv = document.getElementById("userFriendMessageDiv");
    const myUserId = document.getElementById("myUserId");

    const userProfileImage = document.getElementById("userProfileImage");
    const userFullName = document.getElementById("userFullName");

    const model = { UserId: myUserId.innerHTML, OtherUserId: otherUserId }


    $.ajax({
        url: '/Message/Messages', 
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(model),
        success: function (data) {
            userFriendMessageDiv.innerHTML = '';  

            data.$values.forEach((message) => {
                userFriendMessageDiv.innerHTML += `
                    ${message.userId == myUserId.innerHTML ?

                    `
                    <div class="message my-2 d-flex justify-content-start">
                        <img src="${userProfileImage.src}"  class="rounded-circle" style="width: 40px; height: 40px; margin-right: 10px;">
                        <div class="message-content bg-white p-2 rounded shadow-sm flex-grow-1">
                            <strong>${userFullName.innerHTML}</strong>
                            <p>${message.messageStr}</p>
                            <small class="text-muted">${message.dateTime}</small>
                        </div>
                    </div>
                    `
                    :
                    `
                      <div class="message my-2 d-flex justify-content-end">
                        <div class="message-content bg-white p-2 rounded shadow-sm flex-grow-1 text-right">
                            <strong>${otherUserName}</strong>
                            <p>${message.messageStr}</p>
                            <small class="text-muted">${message.dateTime}</small>
                        </div>
                        <img src="${otherProfileUrl}"  class="rounded-circle" style="width: 40px; height: 40px; margin-left: 10px;">
                    </div>
                    `}

                   
                `;
            });
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', status, error);
        }
    });
}


function addMessageEvent(event) {
    event.preventDefault(); 

    const addMessageInput = document.getElementById("addMessageInput");
    const myUserId = document.getElementById("myUserId").innerHTML; 
    const otherUserMessageId = document.getElementById("otherUserMessageId");
    const recipientUserId = otherUserMessageId.innerHTML; 
    const addMessageErroreMessageP = document.getElementById("addMessageErroreMessageP");


    if (addMessageInput.value != "" && otherUserMessageId.innerHTML != "") {

        addMessageErroreMessageP.style.display = "none";
        const model = {
            MessageStr: addMessageInput.value,
            UserId: myUserId,
            RecipientUserId: recipientUserId
        };


        $.ajax({
            url: '/Message/Add',
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify(model),
            success: function (response) {
                console.log('Mesaj başarıyla eklendi:', response);
                addMessageInput.value = '';

                const pOtherUserMessageProfileImage = document.getElementById("pOtherUserMessageProfileImage");
                const pOtherUserMessageUserName = document.getElementById("pOtherUserMessageUserName");
                const userName = document.getElementById("userName");
                const userProfileImage = document.getElementById("userProfileImage");

                InvokeMessageReflashStart(myUserId, recipientUserId, pOtherUserMessageProfileImage.src, pOtherUserMessageUserName.innerHTML);
                InvokeMessageReflashStart(recipientUserId, myUserId, userProfileImage.src, userName.innerHTML);
            },
            error: function (xhr, status, error) {
                console.error('AJAX Hatası:', status, error);
            }
        });
    }
    else {
        addMessageErroreMessageP.style.display = "block";
    }
}





// Message End











// Start Metot Start
async function Start() {
    try {
        await connection.start(); 

    } catch (err) {
        console.error(err);
    }
}

Start();
// Start Metot End

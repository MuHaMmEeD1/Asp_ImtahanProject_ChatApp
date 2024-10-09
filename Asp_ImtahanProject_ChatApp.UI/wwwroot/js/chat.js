




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



});

connection.on("PostUlReflashStart", (tagName) => {


    const searchInput = document.getElementById("searchInput");
    if (searchInput.value == tagName) {
        searchPostIsTagMethod();
    }

});

async function InvokePostUlReflash() {
    const searchInput = document.getElementById("searchInput");
    await connection.invoke("PostUlReflash", searchInput.value);
    console.log("Invoke Ok " + searchInput.value);
}












function updateLabel(input) {
    var label = document.querySelector('label[for="Photo"]');
    if (input.files && input.files.length > 0) {
        label.textContent = input.files[0].name;
    } else {
        label.textContent = "Image Not Selected";
    }
}

function searchPostIsTag(event) {
    event.preventDefault();

    searchPostIsTagMethod();
  
}






function searchPostIsTagMethod() {

    const searchInput = document.getElementById("searchInput");
    const postsUl = document.getElementById("postsUl");
    const userProfileImage = document.getElementById("userProfileImage");
    const myUserId = document.getElementById("myUserId");

    let replyed = false;

    if (searchInput.value.trim() === "") {
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

                    listItem.innerHTML = `
                        <div class="news-feed news-feed-post" style=" margin-bottom: 30px; ">
                            <div class="post-header d-flex justify-content-between align-items-center">
                                <div class="image">
                                    <a href="my-profile.html"><img src="${post.userProfileImageUrl}" class="rounded-circle" alt="image"></a>
                                </div>
                                <div class="info ms-3">
                                    <span class="name"><a href="my-profile.html">${post.userName}</a></span>
                                    <span class="small-text"><a href="#">${post.dateTime}</a></span>
                                </div>
                                ${itIsMyPost ? '' : `
                                <div class="dropdown">
                                    <button class="dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="flaticon-menu"></i></button>
                                    <ul class="dropdown-menu">
                                        <li>
                                            <a class="dropdown-item btn btn-success d-flex align-items-center" href="#">
                                                <i class="flaticon-edit"></i> Add Friend
                                            </a>
                                        </li>
                                    </ul>
                                </div>`}
                            </div>
                            <div class="post-body">
                                <p>${post.text}</p>
                                <div class="post-image">
                                    ${post.imageUrl ? `
                                        <img src="${post.imageUrl}" alt="image" style="width: 100%; height: auto; object-fit: contain;">` : ''}
                                </div>
                                ${post.videoLink ? `
                                    <div class="embed-responsive" style="width: 100%; max-width: 1000px; height: 400px; margin: 0 auto;">
                                        <iframe class="embed-responsive-item" 
                                            style="width: 100%; height: 100%;"
                                            src="${post.videoLink.includes('youtu.be') ?
                                post.videoLink.replace('youtu.be/', 'youtube.com/embed/') :
                                post.videoLink.replace('watch?v=', 'embed/')}" 
                                            frameborder="0" allowfullscreen>
                                        </iframe>
                                    </div>` : ''}
                                <ul class="post-meta-wrap d-flex justify-content-between align-items-center">
                                    <li class="post-react">
                                        <a href="#"><i class="flaticon-like"></i><span>Like</span> <span class="number">${post.likes.$values.length}</span></a>
                                    </li>
                                    <li class="post-comment" onclick="openCloseComments(event,${index})">
                                        <a href="#"><i class="flaticon-comment" ></i>
                                        <span>Comment</span> <span class="number">${post.comments.$values.length}</span>
                                        </a>
                                    </li>
                                </ul>


                               <div id="commentDiv${index}" class="post-comment-list" style="display: block;">

                               </div>


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

    $.ajax({
        url: '/Home/CreatePost',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                $('#createPostForm')[0].reset();
            }
        },
        error: function (xhr, status, error) {
            console.log('An error occurred: ' + error);
        }
    });
}












async function Start() {
    try {
        await connection.start(); 

    } catch (err) {
        console.error(err);
    }
}

Start();

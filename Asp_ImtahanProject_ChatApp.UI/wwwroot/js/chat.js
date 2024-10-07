




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
                        <div class="news-feed news-feed-post">
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
                                    <li class="post-comment">
                                        <a href="#"><i class="flaticon-comment"></i><span>Comment</span> <span class="number">${post.comments.$values.length}</span></a>
                                    </li>
                                </ul>


                                <div id="commentDiv${index}" class="post-comment-list">
                                           
                                  
                                </div>


                                <form class="post-footer">
                                    <div class="footer-image">
                                        <a href="#"><img src="${userProfileImage.src}" class="rounded-circle" alt="image"></a>
                                    </div>
                                    <div class="form-group d-flex align-items-center">
                                        <textarea id="addCommentInput${index}" name="message" class="form-control me-2" placeholder="Write a comment..."></textarea>
                                        <button type="button" class="btn btn-primary" onclick="addCommentFunction(event, ${index}, ${post.postId})">Add Comment</button>
                                    </div>
                                </form>
                            </div>
                        </div>
                    `;
                    postsUl.appendChild(listItem);

                    const commentDiv = document.getElementById(`commentDiv${index}`);
                    commentDiv.innerHTML = "";

                    post.comments.$values.forEach((comment, index) => {
                        commentDiv.innerHTML += `


                              <div class="comment-list">
                                        <div class="comment-image">
                                    <a href="my-profile.html"><img src="${comment.userProfileImageUrl}" class="rounded-circle" alt="image"></a>
                                        </div>
                                        <div class="comment-info">
                                            <h3>
                                                <a href="my-profile.html">${comment.userName}</a>
                                            </h3>
                                            <span>${comment.dateTime}</span>
                                            <p>${comment.text}</p>
                                            <ul class="comment-react">
                                                <li><a href="#">Reply</a></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="more-comments">
                                        <a href="#">More Comments+</a>
                                    </div>

                        `

                    })
                });
            }
        },
        error: function (xhr, status, error) {
            console.error("Error fetching posts:", status, error);
        }
    });
}

function addCommentFunction(event, index, postId) {
    event.preventDefault();
    const addCommentInput = document.getElementById(`addCommentInput${index}`).value;
   


    if (addCommentInput.trim() !== "") {
        $.ajax({
            url: '/Comment/AddComment',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                Text: addCommentInput,
                PostId: postId,
                UserId: myUserId.innerHTML
            }),
            success: function (response) {
                console.log("Comment added successfully");
                document.getElementById(`addCommentInput${index}`).value = '';

               
                InvokePostUlReflash();

            },
            error: function (xhr, status, error) {
                console.error("Error adding comment:", status, error);
            }
        });
    } else {
        alert("Comment cannot be empty!");
    }
}















async function Start() {
    try {
        await connection.start(); 

    } catch (err) {
        console.error(err);
    }
}

Start();

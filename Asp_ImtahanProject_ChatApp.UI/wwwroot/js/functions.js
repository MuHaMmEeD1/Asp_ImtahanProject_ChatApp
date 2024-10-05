function updateLabel(input) {
    var label = document.querySelector('label[for="Photo"]');
    if (input.files && input.files.length > 0) {
        label.textContent = input.files[0].name; 
    } else {
        label.textContent = "Image Not Selected"; 
    }
}
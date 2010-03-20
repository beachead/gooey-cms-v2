function toggleVisibility(div) {
    var obj = document.getElementById(div);
    if (obj) {
        var text = document.getElementById(div + "-text");
        var name = document.getElementById(div + "-name");
        if (obj.style.display == 'none') {
            text.innerHTML = 'Hide';
            obj.style.display = 'block';
            name.style.display = 'none';
        }
        else {
            text.innerHTML = 'Show';  
            obj.style.display = 'none';
            name.style.display = 'block';
        }
    }
}

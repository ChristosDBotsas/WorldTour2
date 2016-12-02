function reveal(id) {
    var f = document.getElementById(id);
    f.style.display = ((f.style.display == 'table-cell') ? 'none' : 'table-cell');
}

function emptyText() {
    var n = document.getElementById("departureCity");
    var s = document.getElementById("destinationCity");
    var t = document.getElementById("adults");
    if (n.value == "") {
        n.style.backgroundColor = "yellow";
        alert("Fill the departure city field!!!!");
    }
    if (s.value == "") {
        s.style.backgroundColor = "yellow";
        alert("Fill the destination city field!!!");
    }
    if (t.value == "") {
        t.style.backgroundColor = "yellow";
        alert("Fill the number of persons!!!!");
    }
}

function clearBox() {
    var n = document.getElementById("departureCity");
    var s = document.getElementById("destinationCity");
    var t = document.getElementById("adults");
    n.style.backgroundColor = "white";
    s.style.backgroundColor = "white";
    t.style.backgroundColor = "white";
}

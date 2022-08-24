<?php
if(!isset($_POST['score'])) {
    echo "Empty Field";
    exit();
}
if(!isset($_POST['username'])) {
    echo "Empty Field";
    exit();
}
if(!isset($_POST['uniqueid'])) {
    echo "Empty Field";
    exit();
}
if(!isset($_POST['flag'])) {
    echo "Empty Field";
    exit();
}

$Score = $_POST['score'];
$Username = $_POST['username'];
$Uniqueid = $_POST['uniqueid'];
$Flag = $_POST['flag'];


if(!empty($Score) && !empty($Username) && !empty($Uniqueid) && !empty($Flag)){
    $Username = substr($Username, 0, 25);
    $Score = intval($Score);
    $conn = new mysqli('localhost', 'root', '', '[REDACTED]');
    
    $Username = $conn->real_escape_string($Username);
    $Score = $conn->real_escape_string($Score);
    $Uniqueid = $conn->real_escape_string($Uniqueid);
    $Flag = $conn->real_escape_string($Flag);


    $sql = "SELECT * FROM leaderboard WHERE Uniqueid = '$Uniqueid'";
    $result = $conn->query($sql);
    if($result->num_rows > 0) {
        $sql = "UPDATE leaderboard SET Score = '$Score', Username = '$Username' WHERE Uniqueid = '$Uniqueid'";
        $result = $conn->query($sql);
        if($result) {
            echo "success";
        } else {
            echo "Error";
        }
    }else {
        $sql = "INSERT INTO leaderboard (Uniqueid, Username, Score, Flag) VALUES ('$Uniqueid', '$Username', '$Score', '$Flag')";
        $result = $conn->query($sql);
        if($result){
            echo "success";
        } else{
            echo "Error";
        }
    }
    
    
    $conn->close();
} else{
    echo "Please fill all the fields";
    exit();
}
?>
<?php
$conn = new mysqli('localhost', 'root', '', '[REDACTED]');
    
$sql = "SELECT * FROM leaderboard ORDER BY Score DESC LIMIT 100";
$result = $conn->query($sql);

if($result->num_rows > 0) {
    $data = array();
    while($row = $result->fetch_assoc()) {
        $obj = new stdClass();
        $obj->Username = $row['Username'];
        $obj->Score = intval($row['Score']);
        $obj->Flag = $row['Flag'];
        $data[] = $obj;
    }

    echo json_encode($data);
} else {
    echo "error";
}

?>
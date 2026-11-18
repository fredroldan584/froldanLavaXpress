<?php
include "config.php";
include "utils.php";
$dbConn = connect($db);

// GET: listar pedidos con el nombre del cliente (o uno solo con ?codigo=)
if ($_SERVER['REQUEST_METHOD'] == 'GET')
{
    if (isset($_GET['codigo']))
    {
        $sql = $dbConn->prepare("SELECT p.*, CONCAT(c.nombre, ' ', c.apellido) AS cliente
                                 FROM pedido p INNER JOIN cliente c ON p.codigo_cliente = c.codigo
                                 WHERE p.codigo=:codigo");
        $sql->bindValue(':codigo', $_GET['codigo']);
        $sql->execute();
        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetch(PDO::FETCH_ASSOC));
        exit();
    }
    else
    {
        $sql = $dbConn->prepare("SELECT p.*, CONCAT(c.nombre, ' ', c.apellido) AS cliente
                                 FROM pedido p INNER JOIN cliente c ON p.codigo_cliente = c.codigo
                                 ORDER BY p.codigo DESC");
        $sql->execute();
        $sql->setFetchMode(PDO::FETCH_ASSOC);
        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetchAll());
        exit();
    }
}

// POST: insertar pedido (el estado queda 'Recibido' por defecto)
if ($_SERVER['REQUEST_METHOD'] == 'POST')
{
    $input = $_POST;
        $sql = "INSERT INTO pedido (codigo_cliente, fecha, servicio, cantidad, total)
            VALUES (:codigo_cliente, :fecha, :servicio, :cantidad, :total)";
    $statement = $dbConn->prepare($sql);
    bindAllValues($statement, $input);
    $statement->execute();
    $codigo = $dbConn->lastInsertId();
    if ($codigo)
    {
        $input['codigo'] = $codigo;
        header("HTTP/1.1 200 OK");
        echo json_encode($input);
        exit();
    }
}

// PUT: actualizar pedido (por ejemplo el estado), datos por la URL
if ($_SERVER['REQUEST_METHOD'] == 'PUT')
{
    $input = $_GET;
    $codigo = $input['codigo'];
    $fields = getParams($input);
    $sql = "UPDATE pedido SET $fields WHERE codigo='$codigo'";
    $statement = $dbConn->prepare($sql);
    bindAllValues($statement, $input);
    $statement->execute();
    header("HTTP/1.1 200 OK");
    exit();
}

// DELETE: eliminar pedido
if ($_SERVER['REQUEST_METHOD'] == 'DELETE')
{
    $statement = $dbConn->prepare("DELETE FROM pedido WHERE codigo=:codigo");
    $statement->bindValue(':codigo', $_GET['codigo']);
    $statement->execute();
    header("HTTP/1.1 200 OK");
    exit();
}
?>
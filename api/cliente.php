<?php
include "config.php";
include "utils.php";
$dbConn = connect($db);

// GET: listar clientes (o uno solo si llega ?codigo=)
if ($_SERVER['REQUEST_METHOD'] == 'GET')
{
    if (isset($_GET['codigo']))
    {
        $sql = $dbConn->prepare("SELECT * FROM cliente WHERE codigo=:codigo");
        $sql->bindValue(':codigo', $_GET['codigo']);
        $sql->execute();
        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetch(PDO::FETCH_ASSOC));
        exit();
    }
    else
    {
        $sql = $dbConn->prepare("SELECT * FROM cliente");
        $sql->execute();
        $sql->setFetchMode(PDO::FETCH_ASSOC);
        header("HTTP/1.1 200 OK");
        echo json_encode($sql->fetchAll());
        exit();
    }
}

// POST: insertar cliente
if ($_SERVER['REQUEST_METHOD'] == 'POST')
{
    $input = $_POST;
    $sql = "INSERT INTO cliente (nombre, apellido, telefono, direccion)
            VALUES (:nombre, :apellido, :telefono, :direccion)";
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

// PUT: actualizar cliente (datos llegan por la URL)
if ($_SERVER['REQUEST_METHOD'] == 'PUT')
{
    $input = $_GET;
    $codigo = $input['codigo'];
    $fields = getParams($input);
    $sql = "UPDATE cliente SET $fields WHERE codigo='$codigo'";
    $statement = $dbConn->prepare($sql);
    bindAllValues($statement, $input);
    $statement->execute();
    header("HTTP/1.1 200 OK");
    exit();
}

// DELETE: eliminar cliente
if ($_SERVER['REQUEST_METHOD'] == 'DELETE')
{
    $statement = $dbConn->prepare("DELETE FROM cliente WHERE codigo=:codigo");
    $statement->bindValue(':codigo', $_GET['codigo']);
    $statement->execute();
    header("HTTP/1.1 200 OK");
    exit();
}
?>
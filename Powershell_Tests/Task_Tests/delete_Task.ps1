function Delete_Task {
    param(
        [string]$LocalhostUrl,
        [string]$ContentType
    )
    $result = Invoke-RestMethod -Method DELETE -ContentType $ContentType -Uri $LocalhostUrl

    return $result
}

$taskForDeletingId = 13
$localhostUrl = "https://localhost:7054/api/Task/" + $taskForDeletingId
$contentType = 'Application/Json'

$response = Delete_Task -ReqBody $requestBody -LocalhostUrl $localhostUrl -ContentType $contentType
$response

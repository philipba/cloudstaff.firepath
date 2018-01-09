# Cloudstaff Firepath
http://cloudstafffirepathapidemo.azurewebsites.net/api/barcode

## API Endpoints

`POST /api/barcode`
- file can be sent via form data

Response:
`[
    {
        "id": "image.png",
        "value": "HELLO WORLD",
        "format": 16
    }
]`

`POST /api/barcodevideo`
- file can be sent via form data

Response:
`
[
    {
        "timeInSeconds": 6,
        "id": null,
        "value": "CRK01-FEX-001-BEGIN",
        "format": 4
    },
    {
        "timeInSeconds": 19,
        "id": null,
        "value": "CRK01-FEX-001-END",
        "format": 4
    }
]
`

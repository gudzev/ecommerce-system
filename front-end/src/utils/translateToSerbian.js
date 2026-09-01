const translations =
{
    // gpu
    "brand": "brend",
    "gpu model": "model grafičkog procesora",
    "vram": "video memorija",
    "memory type": "tip memorije",
    "memory bus": "memorijska magistrala",
    "core clock": "osnovni takt",
    "boost clock": "boost takt",
    "interface": "interfejs",
    "number of fans": "broj ventilatora",
    "tdp": "potrošnja struje",
    "hdmi ports": "HDMI portovi",
    "displayport ports": "DisplayPort portovi",
    "gpu length": "dužina grafičke kartice",

    // cpu
    "socket": "socket",
    "cores": "jezgra",
    "threads": "niti",
    "base clock": "osnovni takt",
    "cache": "keš",
    "integrated graphics": "integrisana grafička",
    "supported memory type": "podržani tip memorije",

    // motherboard
    "chipset": "čipset",
    "form factor": "format",
    "ram slots": "RAM slotovi",
    "maximum ram capacity": "maksimalni kapacitet RAM memorije",
    "maximum ram speed": "maksimalna brzina RAM memorije",
    "m.2 slots": "M.2 slotovi",
    "sata ports": "SATA portovi",
    "pcie slots": "PCIe slotovi",
    "wi-fi": "Wi-Fi",
    "bluetooth": "Bluetooth",
    "lan": "LAN",
    "audio": "audio",
    "usb ports": "USB portovi",

    // ram
    "capacity": "kapacitet",
    "number of modules": "broj modula",
    "capacity per module": "kapacitet po modulu",
    "memory speed": "brzina memorije",
    "latency": "latencija",
    "voltage": "napon",
    "ecc": "ECC",
    "rgb": "RGB",
    "xmp / expo support": "XMP / EXPO podrška",

    // ssd
    "ssd type": "tip SSD-a",
    "read speed": "brzina čitanja",
    "write speed": "brzina pisanja",
    "nand type": "tip NAND memorije",
    "dram cache": "DRAM keš",
    "tbw": "TBW",

    // hdd
    "rotational speed": "brzina obrtanja",

    // power supply
    "power": "snaga",
    "80 plus certification": "80 PLUS sertifikat",
    "modularity": "modularnost",
    "pcie connectors": "PCIe konektori",
    "sata connectors": "SATA konektori",
    "cpu connectors": "CPU konektori",
    "fan size": "veličina ventilatora",

    // case
    "case type": "tip kućišta",
    "supported motherboard form factors": "podržani formati matičnih ploča",
    "maximum gpu length": "maksimalna dužina grafičke kartice",
    "maximum cpu cooler height": "maksimalna visina CPU hladnjaka",
    "2.5 inch drive bays": "2.5-inčna ležišta za diskove",
    "3.5 inch drive bays": "3.5-inčna ležišta za diskove",
    "included fans": "uključeni ventilatori",
    "maximum fans": "maksimalan broj ventilatora",
    "radiator support": "podrška za radijatore",
    "side panel": "bočna stranica",
    "material": "materijal",

    // general case
    "yes": "da",
    "no": "ne"
};

export function translateToSerbian(text, options = { capitalize: false })
{
    const translatedText = translations[text.toLowerCase()] || text;

    if(!options.capitalize)
        return translatedText;

    if(options.capitalize)
    {
        return translatedText.charAt(0).toUpperCase() + translatedText.slice(1);
    }
}
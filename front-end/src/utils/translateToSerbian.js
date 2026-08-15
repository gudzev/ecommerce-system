const translations =
{
    maxGpuLength: "Maksimalna dužina grafičke",
    maxCpuCoolerHeight: "Maksimalna visina hlađenja",
    size: "Veličina",
    weight: "Težina",
    motherboardSize: "Veličina matične ploče",
    dimensions: "Dimenzije",
    cooling: "Hlađenje",
    vram: "VRAM",
    _interface: "Interfejs",
    clockSpeed: "Radni takt",
    read_speed: "Brzina čitanja",
    write_speed: "Brzina pisanja",
    rpm: "Obrtaji u minuti",
    wattage: "Snaga",
    efficiency: "Efikasnost",
    brand: "Brend",
    cores: "Jezgra",
    threads: "Niti",
    l1Cache: "L1 Keš",
    l2Cache: "L2 Keš",
    l3Cache: "L3 Keš",
    capacity: "Kapacitet",
    speed: "Brzina",
    timings: "Latencija",
    type: "Tip",
    form_factor: "Format",
    socket: "Soket",
    ramType: "Tip RAM-a",
    chipset: "Čipset",
    wifi: "Wi-Fi",
    bluetooth: "Bluetooth",
    ramSlots: "Slotovi za RAM",
    m2Slots: "M.2 slotovi",
    sataSlots: "SATA slotovi",
    pcieSlots: "PCIe slotovi"
}

export function translateToSerbian(text)
{
    return translations[text];
}
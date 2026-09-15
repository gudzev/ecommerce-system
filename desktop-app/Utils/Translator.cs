using System;
using System.Collections.Generic;

public class Translator
{
    private static Dictionary<string, string> translations = new Dictionary<string, string>
        {
            // GPU
            { "brand", "brend" },
            { "gpu model", "model grafičkog procesora" },
            { "vram", "video memorija" },
            { "memory type", "tip memorije" },
            { "memory bus", "memorijska magistrala" },
            { "core clock", "osnovni takt" },
            { "boost clock", "boost takt" },
            { "interface", "interfejs" },
            { "number of fans", "broj ventilatora" },
            { "tdp", "potrošnja struje" },
            { "hdmi ports", "HDMI portovi" },
            { "displayport ports", "DP portovi" },
            { "gpu length", "dužina grafičke" },

            // CPU
            { "socket", "socket" },
            { "cores", "jezgra" },
            { "threads", "niti" },
            { "base clock", "osnovni takt" },
            { "cache", "keš" },
            { "integrated graphics", "integrisana grafička" },
            { "supported memory type", "podržani tip memorije" },

            // Motherboard
            { "chipset", "čipset" },
            { "form factor", "format" },
            { "ram slots", "RAM slotovi" },
            { "maximum ram capacity", "max kapacitet RAM-a" },
            { "maximum ram speed", "max brzina RAM-a" },
            { "m.2 slots", "M.2 slotovi" },
            { "sata ports", "SATA portovi" },
            { "pcie slots", "PCIe slotovi" },
            { "wi-fi", "Wi-Fi" },
            { "bluetooth", "Bluetooth" },
            { "lan", "LAN" },
            { "audio", "audio" },
            { "usb ports", "USB portovi" },

            // RAM
            { "capacity", "kapacitet" },
            { "number of modules", "broj modula" },
            { "capacity per module", "kapacitet po modulu" },
            { "memory speed", "brzina memorije" },
            { "latency", "latencija" },
            { "voltage", "napon" },
            { "ecc", "ECC" },
            { "rgb", "RGB" },
            { "xmp / expo support", "XMP / EXPO podrška" },

            // SSD
            { "ssd type", "tip SSD-a" },
            { "read speed", "brzina čitanja" },
            { "write speed", "brzina pisanja" },
            { "nand type", "tip NAND memorije" },
            { "dram cache", "DRAM keš" },
            { "tbw", "TBW" },

            // HDD
            { "rotational speed", "brzina obrtanja" },

            // Power supply
            { "power", "snaga" },
            { "80 plus certification", "80 PLUS sertifikat" },
            { "modularity", "modularnost" },
            { "pcie connectors", "PCIe konektori" },
            { "sata connectors", "SATA konektori" },
            { "cpu connectors", "CPU konektori" },
            { "fan size", "veličina ventilatora" },

            // Case
            { "case type", "tip kućišta" },
            { "supported motherboard form factors", "podržani formati matičnih ploča" },
            { "maximum gpu length", "maksimalna dužina grafičke kartice" },
            { "maximum cpu cooler height", "maksimalna visina CPU hladnjaka" },
            { "2.5 inch drive bays", "2.5-inčna ležišta za diskove" },
            { "3.5 inch drive bays", "3.5-inčna ležišta za diskove" },
            { "included fans", "uključeni ventilatori" },
            { "maximum fans", "maksimalan broj ventilatora" },
            { "radiator support", "podrška za radijatore" },
            { "side panel", "bočna stranica" },
            { "material", "materijal" },

            // General
            { "yes", "da" },
            { "no", "ne" }
        };

    public string TranslateToSerbian(string text, bool capitalize = false)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        string translatedText = translations.TryGetValue(text, out var translation)
            ? translation
            : text;

        if (capitalize)
        {
            return char.ToUpper(translatedText[0]) + translatedText.Substring(1);
        }

        return translatedText;
    }
}
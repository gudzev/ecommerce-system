import "./Footer.css";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFacebook, faLinkedin, faInstagram, faYoutube } from "@fortawesome/free-brands-svg-icons";
import { faPhone, faEnvelope } from "@fortawesome/free-solid-svg-icons";

import { Link } from "react-router-dom";

export function Footer()
{
    return <footer>
        <div className="footer-content">
            <div className="footer-info">
                <Link className="header-img-wrapper footer-img-wrapper" to="/">
                    <img src="/logo.png" className="header-img" alt="Prodavnica logo"/>
                </Link>

                <div className="working-time">
                    <p>Radno vreme kontakt centra</p>
                    <p>Radnim danima: 8 - 21h</p>
                    <p>Reklamacije: 8 - 18h</p>
                    <p>Subotom: 9 - 16h</p>
                    <p>Reklamacije: 10 - 14h</p>
                    <p>Nedeljom: Ne radi</p>
                </div>

                <div className="footer-contact">
                    <p><FontAwesomeIcon icon={faPhone} className="fa-textright" />011-3-713-713</p>
                    <a href="#"><FontAwesomeIcon icon={faEnvelope} className="fa-textright"/>Kontakt</a>
                </div>

                <div className="footer-socials">
                    <p>Pratite nas:</p>

                    <div className="footer-socials-links">

                        <a target="_blank" href="https://www.facebook.com/" className="fa-socials">
                            <FontAwesomeIcon icon={faFacebook} className="fa-icon-2x"/>
                        </a>

                        <a target="_blank" href="https://www.linkedin.com/" className="fa-socials">
                            <FontAwesomeIcon icon={faLinkedin} className="fa-icon-2x"/>
                        </a>

                        <a target="_blank" href="https://www.instagram.com/" className="fa-socials">
                            <FontAwesomeIcon icon={faInstagram} className="fa-icon-2x"/>
                        </a>

                        <a target="_blank" href="https://www.youtube.com/" className="fa-socials">
                            <FontAwesomeIcon icon={faYoutube} className="fa-icon-2x"/>
                        </a>
                    </div>
                </div>
            </div>

            <div className="footer-pages">

                <div className="footer-pages-1">
                    <h3>Upoznaj Prodavnicu</h3>
                    <a href="#">O nama</a>
                    <a href="#">Zaposlenje</a>
                    <a href="#">Prodavnice</a>
                    <a href="#">Podaci o trgovcu</a>
                </div>

                <div className="footer-pages-2">
                    <h3>Online kupovina</h3>
                    <a href="#">Uputstvo za online kupovinu</a>
                    <a href="#">Uslovi online kupovine</a>
                    <a href="#">Ovlašćeni servisi</a>
                    <a href="#">Reklamacije</a>
                    <a href="#">Prava potrošača</a>
                </div>

                <div className="footer-pages-3">
                    <h3>Poručivanje i dostava</h3>
                    <a href="#">Načini plaćanja</a>
                    <a href="#">Načini isporuke</a>
                    <a href="#">Česta pitanja</a>
                    <a href="#">Politika privatnosti</a>
                </div>

                <div className="footer-pages-4">
                    <h3>U centru pažnje</h3>
                    <a href="#">Preuzmi u radnji</a>
                    <a href="#">Robne marke</a>
                    <a href="#">Pravna lica</a>
                    <a href="#">Blog</a>
                </div>

            </div>

            <div className="footer-image">
                <img src="/prodavnica.png" alt="Ljudi sa paketom iz prodavnice" className="footer-img"/>
            </div>
        </div>
    </footer>
}
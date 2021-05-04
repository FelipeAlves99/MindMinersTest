import { useState, useEffect } from 'react';
import { Link } from "react-router-dom";
import { FiLogIn } from "react-icons/fi";

import "./styles.css";

const API_URL = 'https://localhost:5001';

export default function DownloadFile() {
    const [data, setData] = useState([]);
    const [downloadLink, setDownloadLink] = useState([]);

    useEffect(() => {

        fetch(`${API_URL}/File`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        })
            .then(response => {
                return response.json();
            })
            .then((data) => {
                setData(data);
            })
            .catch((error) => {
                console.log(error);
            });
    }, []);

    const fileLink = (e) => {

        console.log("entrei")

        const options = {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
            }
        };

        fetch(`${API_URL}/File/Download?filename=${e}`, options)
            .then(response => response.blob())
            .then((response) => {
                const url = window.URL.createObjectURL(new Blob([response]));
                setDownloadLink(url);
            })
            .catch(error => { console.log('request failed', error); });
    }

    const Historico = ({ links }) => {
        return (
            <div>
                {!links.length ? (
                    <h1>Não há links</h1>
                ) : (
                    links.map((fileName) => {
                        return (
                            <div key={fileName}>
                                <p>{fileName}</p>
                                <a href={downloadLink} download="legenda-com-offset.srt" onClick={() => fileLink(fileName) }> Download </a>
                            </div>
                        );
                    })
                )}
            </div>
        );
    };


    return (
        <div className="downloadFile-container">
            <header>
                <h1>Histórico de arquivos</h1>
            </header>
            <div className="historico">
                <Historico links={data} />
            </div>
            <Link className="file-list" to="/">
                <FiLogIn size={16} color="#e02041" />
                    Voltar
            </Link>
        </div>
    );
}
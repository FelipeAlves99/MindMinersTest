import { useState } from 'react';
import InputMask from "react-input-mask";
import { Link } from "react-router-dom";
import { FiLogIn } from "react-icons/fi";

import "./styles.css";

const API_URL = 'https://localhost:5001';

export default function UploadFile() {
    const [file, setFile] = useState(null);
    const [offset, setOffset] = useState("");
    const [downloadLink, setDownloadLink] = useState({});
    const [responseLink, setResponseLink] = useState(false);

    const OnChangeHandler = (event) => {
        setFile(event.target.files[0])
    }

    const OnChangeInputMask = (e) => {
        setOffset(e.target.value)
    }

    const sendFile = (e) => {
        e.preventDefault();

        const data = new FormData()
        data.append('srtfile', file)
        data.append('offset', offset)

        console.log(data);

        const options = {
            method: "POST",
            body: data
        };
        fetch(`${API_URL}/File`, options)
            .then(response => response.blob())
            .then((response) => {
                const url = window.URL.createObjectURL(new Blob([response]));
                setDownloadLink(url);
                setResponseLink(true);
            })
            .catch(error => { console.log('request failed', error); });
    }

    return (
        <div className="uploadFile-container">
            <section className="form">
                <form>
                    <h1>Faça seu upload</h1>
                    <input placeholder="Arquivo .srt" type='file' name='file' onChange={OnChangeHandler} />
                    <InputMask mask="99:99:99,999" placeholder="00:00:00,000" value={offset} onChange={OnChangeInputMask} />
                    <button className="button" type="submit" onClick={sendFile}>
                        Adicionar offset
                    </button>
                    <Link className="file-list" to="/filelist">
                        <FiLogIn size={16} color="#e02041" />
                        Abrir histórico
                    </Link>
                </form>
                {responseLink && <a href={downloadLink} download="legenda-com-offset.srt">Download</a>}            
            </section>
        </div>
    );
}
import './styles.css'
import {API_BASE_URL} from '../api-config.js'
import {Link} from 'react-router-dom'
import {useState} from "react";

const HomePage = () => {
    const [loading, setLoading] = useState(false);

    const sendRequest = async (url: string, method: string) => {
        if (loading) return;

        setLoading(true);
        try {
            const response = await fetch(url, { method });
            const data = await response.json();
            alert(data);
        } catch (error) {
            alert("Klaida siunčiant užklausą");
            console.error(error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="flex flex-col items-center mt-16 gap-10">
            <h2 className="font-bold text-3xl">Funkcijos:</h2>
            <div className="flex flex-row gap-10">

                <button
                    className="function-button disabled:opacity-50 disabled:cursor-not-allowed disabled:pointer-events-none"
                    onClick={() => sendRequest(`${API_BASE_URL}/customers/import`, "POST")}
                    disabled={loading}
                >
                    Importuoti klientus
                </button>

                <button
                    className="function-button disabled:opacity-50 disabled:cursor-not-allowed disabled:pointer-events-none"
                    onClick={() => sendRequest(`${API_BASE_URL}/customers/post-codes`, "PUT")}
                    disabled={loading}
                >
                    Atnaujinti pašto indeksus
                </button>

                <Link
                    to="/clients"
                    className={`function-button ${loading ? 'pointer-events-none opacity-50' : ''}`}
                >
                    Klientų sąrašas
                </Link>
            </div>
        </div>
    );
};

export default HomePage;
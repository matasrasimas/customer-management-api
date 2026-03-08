import {useEffect, useState} from "react";
import {API_BASE_URL} from "../api-config.js";
import type {Client} from "../types.ts";
import GoBackButton from "./go-back-button";

const ClientsPage = () => {
    const [clients, setClients] = useState<Client[]>([])
    const [loading, setLoading] = useState<boolean>(true)

    useEffect(() => {
        const fetchClients = async () => {
            try {
                const response = await fetch(`${API_BASE_URL}/customers/`, {
                    method: "GET"
                });

                const data = await response.json();
                setClients(data);
            } catch (error) {
                alert("Klaida siunčiant užklausą");
                console.error(error);
            } finally {
                setLoading(false);
            }
        }

        fetchClients();
    }, [])

    if (loading) {
        return (
            <div className="flex flex-col items-center mt-15">
                <GoBackButton/>
                <h2 className="text-2xl font-bold mb-4">Kraunamas klientų sąrašas...</h2>
            </div>
        );
    }

    if (clients.length === 0) {
        return (
            <div className="flex flex-col items-center mt-15">
                <GoBackButton/>
                <h2 className="text-2xl font-bold mb-4">Klientų sąrašas yra tuščias</h2>
            </div>
        );
    }

    return (
        <div className="flex flex-col items-center mt-15 gap-5">
            <GoBackButton/>
            <h2 className="text-2xl font-bold mb-4">Klientų sąrašas:</h2>
            <div className="overflow-x-auto">
                <table className="min-w-full border border-gray-300">
                    <thead className="bg-gray-100">
                    <tr>
                        <th className="py-2 px-4 border-b text-center text-black">Pavadinimas</th>
                        <th className="py-2 px-4 border-b text-center text-black">Adresas</th>
                        <th className="py-2 px-4 border-b text-center text-black">Pašto indeksas</th>
                    </tr>
                    </thead>
                    <tbody>
                    {clients.map((client) => (
                        <tr key={client.id}>
                            <td className="py-2 px-4 border-b text-center">{client.name}</td>
                            <td className="py-2 px-4 border-b text-center">{client.address}</td>
                            <td className="py-2 px-4 border-b text-center">{client.postCode ?? "-"}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default ClientsPage;
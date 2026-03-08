import {Routes, Route} from 'react-router-dom'
import HomePage from "./home";
import ClientsPage from "./clients";

const App = () => {
    return (
        <div className="flex flex-col items-center">
            <Routes>
                <Route path="/" element={<HomePage/>}/>
                <Route path="/clients" element={<ClientsPage/>}/>
            </Routes>
        </div>
    );
};


export default App

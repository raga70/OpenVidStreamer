import './App.css';

import {BrowserRouter as Router, Route, Routes,} from "react-router-dom";
import LoginPageComp from "./Pages/Login/LoginPage.tsx";
import UploadPage from "./Pages/Upload/UploadPage.tsx";
import HomePage from "./Pages/VideoRecomendations/HomePage.tsx";
import {useEffect, useState} from "react";
import {useStoreState} from "../persistenceProvider.ts";
import {loadConfig} from "../configProvider.ts";

function App() {

    const authToken = useStoreState("authToken");
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false)

    useEffect(() => {
        const loadConf = async () => {
            await loadConfig()
        }
    
        loadConf();
        console.log(authToken)
    if (authToken != null && authToken != "") {
        console.log("authToken is not null")
        setIsLoggedIn(true)
    }
    }, []);

    return (
        <>
            {!isLoggedIn ? <LoginPageComp/> :
                <Router>
                    <Routes> {/* Updated from Switch to Routes */}
                        <Route path="/login" element={<LoginPageComp/>}/> {/* Updated syntax for Route */}
                        <Route path="/upload" element={<UploadPage/>}/> {/* Updated syntax for Route */}
                        <Route path="/home" element={<HomePage/>}/>
                    </Routes>
                </Router>
            }
        </>
    );
}

export default App;

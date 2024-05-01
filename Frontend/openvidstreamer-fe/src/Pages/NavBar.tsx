import { Link } from 'react-router-dom';
import {dispatch} from "../persistenceProvider.ts";
import toast from "react-hot-toast";


const Navbar = () => {
    

    const handleLogout = () => {
        
      
            dispatch({type: 'setAuthToken', authToken: null});
            setTimeout(() => {
                window.location.reload();
            }, 2000);
       
        toast.promise(new Promise(r => setTimeout(r, 2500)), {loading: "please wait...", success: "", error: "Failed to logout, please try again later"});
    };

    const navbarStyle = {
        backgroundColor: 'rgba(51, 51, 51, 0.8)', // Semi-transparent black
        color: 'white',
        padding: '10px 20px',
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        backdropFilter: 'blur(10px)', // Blur effect for the frosted glass look
        // borderBottom: '1px solid rgba(255, 255, 255, 0.2)', // Optional: subtle border to enhance visibility
    };

    const linkStyle = {
        color: 'white',
        textDecoration: 'none',
        padding: '0 10px',
    };

    const ulStyle = {
        listStyleType: 'none',
        display: 'flex',
        alignItems: 'center',
        padding: '0',
    };

    const buttonStyle = {
        backgroundColor: 'red',
        color: 'white',
        border: 'none',
        padding: '5px 15px',
        cursor: 'pointer',
        borderRadius: '5px' // Rounded corners for the button
    };

    return (
        <nav style={navbarStyle}>
            <ul style={ulStyle}>
                <li><Link to="/home" style={linkStyle}>Home</Link></li>
                <li><Link to="/upload" style={linkStyle}>Upload</Link></li>
            </ul>
            <button onClick={handleLogout} style={buttonStyle}>Logout</button>
        </nav>
    );
};

export default Navbar;

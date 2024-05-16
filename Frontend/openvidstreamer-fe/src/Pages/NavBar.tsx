import {Link, useNavigate} from 'react-router-dom';
import {dispatch, useStoreState} from "../persistenceProvider.ts";
import toast from "react-hot-toast";
import { Box, Button, Flex, Input, InputGroup, InputRightElement, Link as ChakraLink,Text } from '@chakra-ui/react';
import {useState} from "react";

const Navbar = () => {
    

    const handleLogout = () => {
        
      
            dispatch({type: 'setAuthToken', authToken: null});
            setTimeout(() => {
                window.location.reload();
            }, 2000);
       
        toast.promise(new Promise(r => setTimeout(r, 2500)), {loading: "please wait...", success: "", error: "Failed to logout, please try again later"});
    };


    const [searchQuery, setSearchQuery] = useState('');
    const navigate = useNavigate();

    const account = useStoreState('accountData');
    
    const handleSearch = () => {
        navigate('/search', { state: { searchQuery } });
    };
    

    const navbarStyle = {
        backgroundColor: '#eeeef0', // Semi-transparent black
        color: 'black',
        padding: '10px 20px',
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        backdropFilter: 'blur(10px)', // Blur effect for the frosted glass look
         borderBottom: '1px solid rgba(200, 200, 200, 1)', // Optional: subtle border to enhance visibility
       
    };

    const linkStyle = {
        color: 'black',
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

        <Box as="nav" bg="white" color="gray.800" p={4} boxShadow="0 4px 6px rgba(0, 0, 0, 0.1)">
            <Flex align="center" justify="space-between">
                <Box>
                    <ChakraLink as={Link} to="/home" m={2} style={{ textDecoration: 'none' }} _hover={{ textDecoration: 'none' }}>

                    Home
                    </ChakraLink>
                    <ChakraLink as={Link} to="/upload" m={2} style={{ textDecoration: 'none' }} _hover={{ textDecoration: 'none' }}>
                        Upload
                    </ChakraLink>
                    <ChakraLink as={Link} to="/account" m={2} style={{ textDecoration: 'none' }} _hover={{ textDecoration: 'none' }}>
                        Account
                    </ChakraLink>
                </Box>
                <InputGroup size="md" w="30%">
                    <Input
                        pr="4.5rem"
                        type="text"
                        placeholder="Search..."
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                        onKeyPress={(event) => {
                            if (event.key === 'Enter') {
                                handleSearch();
                            }
                        }}
                    />
                    <InputRightElement width="4.5rem">
                        <Button boxShadow="1px 1px 1px rgba(0, 0, 0, 0.1)" style={{backgroundColor:"#fafafa"}} h="1.75rem" size="sm" onClick={handleSearch}>
                            🔎
                        </Button>
                    </InputRightElement>
                </InputGroup>
                
                <div style={{display:"flex", alignItems:"center",justifyContent:"center"}}>
                    <Text style={{margin:15, marginBottom:0,marginTop:0}}>{account.email}</Text>
                <Button onClick={handleLogout} colorScheme="red" variant="solid">
                    Logout
                </Button>
                </div>
            </Flex>
        </Box>
    );
};

export default Navbar;

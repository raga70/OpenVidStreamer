
import { Box, Button, Text, VStack, useColorModeValue } from '@chakra-ui/react';

const PaymentProcessedPage = () => {
   
    const bgColor = useColorModeValue('gray.100', 'gray.700');
    const textColor = useColorModeValue('black', 'white');

    const handleBackToAccount = () => {
       window.location.replace("/account"); // Adjust the route as necessary
    };

    return (
        <VStack
            spacing={4}
            justify="center"
            align="center"
            height="100vh"
            bg={bgColor}
        >
            <Box p={5} shadow="md" borderWidth="1px" borderRadius="lg" align={"center"} justify={"center"}>
                <Text fontSize="2xl" color={textColor}>
                    Payment Processed Successfully!
                </Text>
                <Text mt={4} fontSize="md" color={textColor}>
                    Your transaction has been completed.
                </Text>
                <br/>
            <Button  colorScheme="blue" onClick={handleBackToAccount}>
                Back to Account
            </Button>
            </Box>
        </VStack>
    );
};

export default PaymentProcessedPage;

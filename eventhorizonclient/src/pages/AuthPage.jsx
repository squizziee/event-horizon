import { useContext, useState } from "react";
import { UserContext } from "../App";
import { Box, Button, Card, CircularProgress, Container, Grid2, Stack, Tab, TextField, Typography } from "@mui/material";
import { TabContext, TabList, TabPanel } from "@mui/lab";
import axiosClient from "../tools/axiosConfig";
import axios from "axios";

function AuthPage() {
    const { user, setUser } = useContext(UserContext)
    const [tab, setTab] = useState("1")



    function handleTabChange(e, value) {
        setTab(value);
    }

    function handleLogin(formData) {
        console.log(formData);

        const tmp = new FormData();
        tmp.append("Email", formData.get("Email"))
        tmp.append("Password", formData.get("Password"))

        axiosClient({
            method: "POST",
            url: "user/login",
            data: tmp,
            headers: { "Content-Type": "multipart/form-data" }
        })
            .then(_ => {
                successRedirect();
            })
            .catch(err => {
                console.log(err);
            })
    }

    function handleRegister(formData) {
        axiosClient({
            method: "POST",
            url: "user/register",
            data: formData,
            headers: { "Content-Type": "multipart/form-data" }
        })
            .then(_ => {
                successRedirect();
            })
            .catch(err => {
                console.log(err);
            })
    }

    function handleLogout(formData) {
        axiosClient({
            method: "POST",
            url: "user/logout",
        })
            .then(_ => {
                successRedirect(true);
            })
            .catch(err => {
                console.log(err);
            })
    }

    function successRedirect(logout = false) {
        if (logout) window.location.href = "/auth"
        else window.location.href = "/catalog"
    }

    return (

        <Container maxWidth="sm" sx={{ margin: "50px auto" }}>
            <TabContext value={user ? "3" : tab}>
                <Box>
                    <TabList onChange={handleTabChange} aria-label="lab API tabs example" centered>
                        {
                            user ?
                                <Tab disabled label="Login" value="1" />
                                :
                                <Tab label="Login" value="1" />
                        }

                        {
                            user ?
                                <Tab disabled label="Register" value="2" />
                                :
                                <Tab label="Register" value="2" />
                        }


                        {
                            user ?
                                <Tab label="Log out" value="3" />
                                :
                                <Tab disabled label="Log out" value="3" />
                        }

                    </TabList>
                    <TabPanel value="1">
                        <Card sx={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)" }}>
                            <Stack direction="column" p={2}>
                                <Typography variant="h5" sx={{ marginBottom: 1 }}>Login</Typography>
                                <Stack direction="column" spacing={1.5}>
                                    <form action={handleLogin}>
                                        <Stack direction="column" spacing={1.5}>
                                            <TextField
                                                required
                                                size="small"
                                                label="Email"
                                                name="Email"
                                            />
                                            <TextField
                                                required
                                                size="small"
                                                type="password"
                                                label="Password"
                                                name="Password"
                                            />
                                            <Button type="submit" variant="contained" disableElevation>
                                                Proceed
                                            </Button>
                                        </Stack>
                                    </form>
                                </Stack>
                            </Stack>

                        </Card>
                    </TabPanel>
                    <TabPanel value="2">
                        <Card sx={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)" }}>
                            <Stack direction="column" p={2}>
                                <Typography variant="h5" sx={{ marginBottom: 1 }}>Register</Typography>
                                <Stack direction="column" spacing={1.5}>
                                    <form action={handleRegister}>
                                        <Stack direction="column" spacing={1.5}>
                                            <TextField
                                                required
                                                size="small"
                                                label="Email"
                                                name="Email"
                                            />
                                            <TextField
                                                required
                                                size="small"
                                                type="password"
                                                label="Password"
                                                name="Password"
                                            />
                                            <TextField
                                                required
                                                size="small"
                                                label="First name"
                                                name="FirstName"
                                            />
                                            <TextField
                                                required
                                                size="small"
                                                label="Last name"
                                                name="LastName"
                                            />
                                            <TextField
                                                required
                                                type="date"
                                                size="small"
                                                name="DateOfBirth"
                                            />
                                            <Button type="submit" variant="contained" disableElevation>
                                                Proceed
                                            </Button>
                                        </Stack>
                                    </form>


                                </Stack>
                            </Stack>

                        </Card>
                    </TabPanel>
                    <TabPanel value="3">
                        {
                            user ?
                                <Card sx={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)" }}>
                                    <Stack direction="column" p={2}>
                                        <Typography variant="h5" sx={{ marginBottom: 1 }}>Logged in as {user.firstName} {user.lastName}</Typography>
                                        <Stack direction="column" spacing={1.5}>
                                            <form action={handleLogout}>
                                                <Stack direction="column" spacing={1.5}>
                                                    <Button type="submit" variant="contained" disableElevation>
                                                        Log out
                                                    </Button>
                                                </Stack>
                                            </form>
                                        </Stack>
                                    </Stack>

                                </Card>
                                :
                                <div></div>
                        }

                    </TabPanel>
                </Box>
            </TabContext>

        </Container>
    );
}

export default AuthPage;
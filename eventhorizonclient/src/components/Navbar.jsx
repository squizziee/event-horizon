import { useContext } from "react";
import { UserContext } from "../App";
import { Avatar, Box, Button, Container, Grid2, IconButton, Link, Stack, Typography } from "@mui/material";
import { Build, CelebrationOutlined } from "@mui/icons-material";

function Navbar() {
    const { user, setUser } = useContext(UserContext)

    return (
        <Box width="100%" sx={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)" }} p={1}>
            <Container maxWidth="lg" sx={{ margin: "0 auto" }}>
                <Grid2 container alignItems="center" justifyContent="space-between">
                    <Grid2 item>
                        <Stack direction="row" alignItems="center" spacing={4}>
                            <CelebrationOutlined sx={{ fontSize: 40 }} />
                            <Link href="/profile" underline="none">
                                <Typography sx={{ color: "black", fontWeight: "bold" }}>Profile</Typography>
                            </Link>
                            <Link href="/" underline="none">
                                <Typography sx={{ color: "black", fontWeight: "bold" }}>Catalog</Typography>
                            </Link>
                            {
                                user && user.role == "Admin" ?
                                    <Link href="/admin" underline="none">
                                        <Typography sx={{ color: "black", fontWeight: "bold" }}>Administration</Typography>
                                    </Link>
                                    : null
                            }
                        </Stack>

                    </Grid2>
                    <Grid2 item>
                        {
                            user ?
                                <div>
                                    <Stack direction="row" spacing={2}>
                                        <Stack direction="column" justifyContent="center">
                                            <Typography sx={{ m: 0, lineHeight: 1.5 }} variant="overline">{user.firstName} {user.lastName}</Typography>
                                            <Typography sx={{ m: 0, lineHeight: 1 }} color="textSecondary" variant="caption">{user.email}</Typography>
                                        </Stack>
                                        <Avatar sx={{ bgcolor: "ButtonText", cursor: "pointer" }} onClick={() => window.location.href = "/auth"}>
                                            {
                                                user.role == "Admin" ?
                                                    <div>
                                                        <Build sx={{ fontSize: 20 }} />
                                                    </div>
                                                    :
                                                    <div>
                                                        {user.firstName[0].toUpperCase()}
                                                        {user.lastName[0].toUpperCase()}
                                                    </div>
                                            }

                                        </Avatar>
                                    </Stack>

                                </div>
                                :
                                <div>
                                    <Stack direction="row" spacing={2}>
                                        <IconButton></IconButton>
                                        <Stack direction="column" justifyContent="center">
                                            <Typography sx={{ m: 0, lineHeight: 1.5 }} variant="overline">Not authenticated</Typography>
                                        </Stack>
                                        <Avatar sx={{ bgcolor: "ButtonText", cursor: "pointer" }} onClick={() => window.location.href = "/auth"}>
                                            ?
                                        </Avatar>
                                    </Stack>
                                </div>
                        }
                    </Grid2>
                </Grid2>

            </Container>
        </Box>
    );
}

export default Navbar;
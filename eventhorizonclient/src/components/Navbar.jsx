import { useContext } from "react";
import { UserContext } from "../App";
import { Avatar, Box, Button, Container, Grid2, Stack, Typography } from "@mui/material";
import { CelebrationOutlined, CelebrationRounded } from "@mui/icons-material";

function Navbar() {
    const { user, setUser } = useContext(UserContext)

    return (
        <Box width="100%" sx={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)" }} p={1}>
            <Container maxWidth="lg" sx={{ margin: "0 auto" }}>
                <Grid2 container alignItems="center" justifyContent="space-between">
                    <Grid2 item>
                        <CelebrationOutlined sx={{ fontSize: 40 }} />
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
                                        <Avatar sx={{ bgcolor: "ButtonText", cursor: "pointer" }} onClick={() => window.location.href = "/profile"}>
                                            {user.firstName[0].toUpperCase()}
                                            {user.lastName[0].toUpperCase()}
                                        </Avatar>
                                    </Stack>

                                </div>
                                :
                                <div>

                                </div>
                        }
                    </Grid2>
                </Grid2>

            </Container>
        </Box>
    );
}

export default Navbar;
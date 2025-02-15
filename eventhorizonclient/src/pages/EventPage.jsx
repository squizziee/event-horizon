import { useContext, useEffect, useState } from "react";
import { UserContext } from "../App";
import { useParams } from "react-router-dom";
import { Backdrop, Button, Card, CardActions, CircularProgress, Container, Grid2, Stack, Typography } from "@mui/material";
import axiosClient from "../tools/axiosConfig";

import 'swiper/css/bundle'
import { A11y, Navigation, Pagination, Scrollbar } from "swiper/modules";
import { Swiper, SwiperSlide } from "swiper/react";
import formatTimestamp from "../tools/dateFormatter";
import zIndex from "@mui/material/styles/zIndex";

function EventPage() {
    const { id } = useParams()
    const { user, setUser } = useContext(UserContext)
    const [event, setEvent] = useState()
    const [signupOpen, setSignupOpen] = useState()
    const [entries, setEntries] = useState()

    useEffect(() => {
        axiosClient
            .get(`events/${id}`)
            .then(response => response.data)
            .then(data => {
                console.log(data.event);
                setEvent(data.event)
            })
            .catch(err => {
                console.log(err);
            })
    }, [])

    useEffect(() => {
        axiosClient
            .get(`entries/event/${id}`)
            .then(response => response.data)
            .then(data => {
                console.log(data.entries);
                setEntries(data.entries)
            })
            .catch(err => {
                console.log(err);
            })
    }, [])

    function openSignup() {
        setSignupOpen(true);
    }

    function closeSignup() {
        setSignupOpen(false);
    }

    function submitEntry() {
        axiosClient({
            method: "POST",
            url: `entries/event/${id}`
        })
            .then(_ => {
                window.location.reload();
            })
            .catch(err => {
                console.log(err);
            })
            .finally(() => {
                closeSignup();
            })

    }

    return (
        <Container maxWidth="md" style={{ margin: "50px auto" }}>
            {
                event ?
                    <Stack direction="column">
                        <Typography textAlign="center" variant="h2" sx={{ mb: "50px" }}>{event.name}</Typography>
                        <Swiper
                            style={{ maxWidth: "100%", maxHeight: "500px" }}
                            id="sadadasd"
                            modules={[Navigation, Pagination, Scrollbar, A11y]}
                            spaceBetween={0}
                            slidesPerView={1}
                            scrollbar={{ draggable: true }}
                            direction="vertical"
                        >
                            {
                                event.imageUrls.map((url, index) => (
                                    <SwiperSlide key={index} style={{ height: "100%", display: "flex", justifyContent: "center" }}>
                                        <img src={url} alt="event image" height="100%" />
                                    </SwiperSlide>
                                ))
                            }
                        </Swiper>

                        <Grid2 container spacing={2}>
                            <Grid2 item size={7} spacing={2}>
                                <Card sx={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)", mt: 2 }}>
                                    <Stack direction="column" p={2} useFlexGap>
                                        <Stack direction="row" useFlexGap>
                                            <Typography variant="h6">Description</Typography>
                                        </Stack>

                                        <Typography variant="body" color="GrayText">{event.description}</Typography>
                                    </Stack>
                                </Card>
                            </Grid2>
                            <Grid2 item size={5}>
                                <Card sx={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)", mt: 2 }}>
                                    <Stack direction="column" p={2} useFlexGap>
                                        <Stack direction="row" useFlexGap>
                                            <Typography variant="h6">Info</Typography>
                                        </Stack>

                                        <Typography variant="body" color="GrayText">Held on: {formatTimestamp(event.dateTime)}</Typography>
                                        <Typography variant="body" color="GrayText">Address: {event.address}</Typography>
                                        <Typography variant="body" color="GrayText">Category: {event.category.name}</Typography>
                                        <Typography variant="body" color="GrayText">
                                            Participants:&nbsp;
                                            <Typography variant="body" color={event.currentParticipantCount >= event.maxParticipantCount ? "error" : "primary"}>
                                                {event.currentParticipantCount} of {event.maxParticipantCount}
                                            </Typography>

                                        </Typography>
                                    </Stack>
                                </Card>
                            </Grid2>
                            <Grid2 item size={12}>
                                <Card sx={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)" }}>
                                    <Stack direction="column" p={2} useFlexGap>
                                        <Button variant="outlined" onClick={openSignup}>Sign up</Button>
                                    </Stack>
                                </Card>
                            </Grid2>
                        </Grid2>

                        <Typography textAlign="center" variant="h2" sx={{ mb: "50px", marginTop: "50px" }}>Participants</Typography>

                        {
                            entries ?
                                <Grid2 container spacing={2}>
                                    {
                                        entries.map((e, index) => (
                                            <Grid2 item key={index} size={12}>
                                                <ParticipantComponent entry={e} />
                                            </Grid2>
                                        ))
                                    }

                                </Grid2>
                                :
                                <div></div>
                        }

                    </Stack>
                    :
                    <div>
                        <CircularProgress />
                    </div>
            }
            {
                event ?
                    <Backdrop
                        sx={{ zIndex: 99999 }}
                        open={signupOpen}
                    >
                        <Container maxWidth="sm">
                            <Card sx={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)" }}>
                                <Stack direction="column" p={2} minWidth="md">
                                    <Typography variant="h6">Sign up for {event.name}?</Typography>
                                    <Typography variant="body">Your personal data will be processed; your name will be displayed at the bottom of the page.</Typography>
                                </Stack>
                                <CardActions>
                                    <Button disableElevation onClick={submitEntry}>Sign up</Button>
                                    <Button color="error" onClick={closeSignup}>Decline</Button>
                                </CardActions>
                            </Card>
                        </Container>

                    </Backdrop>
                    :
                    <div></div>
            }


        </Container>
    );
}

function ParticipantComponent({ entry }) {
    return (
        <Card sx={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)" }}>
            <Grid2 container p={2}>
                <Grid2 item>
                    <Typography variant="h6">
                        {entry.user.firstName} {entry.user.lastName}&nbsp;–&nbsp;
                    </Typography>
                </Grid2>
                <Grid2 item>
                    <Typography variant="h6">
                        {formatTimestamp(entry.submissionDate)}
                    </Typography>
                </Grid2>
            </Grid2>
        </Card>
    );
}

export default EventPage;
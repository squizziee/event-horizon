import { useContext, useEffect, useState } from "react";
import { UserContext } from "../App";
import axiosClient from "../tools/axiosConfig";
import { Button, Card, CardContent, CardMedia, Chip, CircularProgress, Container, Grid2, Pagination, Rating, Stack, Typography } from "@mui/material";
import imagePlaceholder from "../misc/event_placeholder.bmp";

function EventCatalogPage() {
    const { user, setUser } = useContext(UserContext)
    const [events, setEvents] = useState(null)
    const [pageNumber, setPageNumber] = useState(0)
    const [totalPages, setTotalPages] = useState(0)

    function changePage(e, value) {
        console.log(value);

        setPageNumber(value - 1)
    }

    useEffect(() => {
        axiosClient
            .get(`events?PageNumber=${pageNumber}`)
            .then(response => response.data)
            .then(data => {
                console.log(data);
                setPageNumber(data.pageNumber);
                setTotalPages(data.totalPages)
                setEvents(data.events)
            })
            .catch(err => {
                console.log(err);
            })
    }, [pageNumber])

    return (
        <Container maxWidth="lg" style={{ margin: "50px auto" }}>
            {
                events ?
                    <div>
                        <Grid2 container spacing={2}>
                            {
                                events.map((e, index) => (
                                    <Grid2 item size={6}>
                                        <EventComponent event={e} key={index} />
                                    </Grid2>

                                ))
                            }
                        </Grid2>
                        <Grid2 container justifyContent={'center'}>
                            <Grid2 item>
                                <Pagination style={{ paddingTop: "50px" }} count={totalPages} page={pageNumber + 1} onChange={changePage} />
                            </Grid2>

                        </Grid2>

                    </div>
                    :
                    <div>
                        <CircularProgress />
                    </div>
            }
        </Container>
    )
}

function EventComponent({ event }) {
    var labelColor = event.currentParticipantCount == event.maxParticipantCount ?
        "error" : "primary";

    return (
        <Card style={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)" }}>
            <CardMedia
                style={{ maxHeight: "200px" }}
                component="img"
                alt=""
                image={
                    event.imageUrls.length != 0 ?
                        event.imageUrls[0]
                        :
                        imagePlaceholder
                }
            />
            <Stack direction="row" alignItems="center" spacing={3} p={2} useFlexGap>
                <Stack direction="column" spacing={0.5} useFlexGap>
                    <Typography variant="h5">{event.name}</Typography>
                    <Typography variant="body2" sx={{ color: "GrayText" }}>{event.description}</Typography>
                    <Stack direction="row" spacing={1} useFlexGap alignItems={'center'} justifyContent={"space-between"}>
                        <Chip
                            variant="outlined"
                            label={`${event.currentParticipantCount} of ${event.maxParticipantCount}`}
                            color={labelColor}
                        />
                        <Button>Overview</Button>
                    </Stack>
                </Stack>
            </Stack>
        </Card>
    );
}

export default EventCatalogPage;
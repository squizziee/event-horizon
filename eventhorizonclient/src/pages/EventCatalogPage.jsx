import { useContext, useEffect, useState } from "react";
import { UserContext } from "../App";
import axiosClient from "../tools/axiosConfig";
import { Box, Button, Card, CardMedia, Chip, CircularProgress, Container, FormControl, FormHelperText, Grid2, MenuItem, Pagination, Stack, TextField, Typography } from "@mui/material";
import imagePlaceholder from "../misc/event_placeholder.bmp";
import { DatePicker } from "@mui/lab";
import Select from "react-select";

function EventCatalogPage() {
    const { user, setUser } = useContext(UserContext)
    const [events, setEvents] = useState(null)
    const [categories, setCategories] = useState(null)
    const [pageNumber, setPageNumber] = useState(0)
    const [totalPages, setTotalPages] = useState(0)
    const [searchParams, setSearchParams] = useState("")

    function changePage(e, value) {
        console.log(value);

        setPageNumber(value - 1)
    }

    function onResultsReceived(data, params) {
        setPageNumber(data.pageNumber);
        setTotalPages(data.totalPages)
        setEvents(data.events)
        setSearchParams(params)
    }

    useEffect(() => {
        axiosClient
            .get(`events/search?${searchParams == "" ? "" : `${searchParams}&`}PageNumber=${pageNumber}`)
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

    useEffect(() => {
        axiosClient
            .get(`category?PageNumber=0&DoNotPaginate=true`)
            .then(response => response.data)
            .then(data => {
                const mapped =
                    data.categories.map(c => ({ value: c.id, label: c.name }))

                console.log(mapped);
                setCategories(mapped)
            })
            .catch(err => {
                console.log(err);
            })
    }, [])

    return (
        <Container maxWidth="lg" style={{ margin: "50px auto" }}>
            {
                events && categories ?
                    <div>
                        <SearchComponent categories={categories} onResultsReceived={onResultsReceived} />
                        <Grid2 container spacing={2}>
                            {
                                events.map((e, index) => (
                                    <Grid2 item size={6} key={index}>
                                        <EventComponent event={e} />
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
            <Stack direction="column" spacing={0.5} p={2} useFlexGap>
                <Stack direction="row" spacing={2} useFlexGap alignItems='center'>
                    <Typography variant="h5">{event.name}</Typography>
                    <Chip
                        size="small"
                        variant="filled"
                        label={`${event.category.name}`}
                        color='default'
                    />
                </Stack>

                <Typography variant="body2" sx={{ color: "GrayText" }}>{event.description}</Typography>
                <Typography variant="body2" sx={{ color: "GrayText" }}>Address: {event.address}</Typography>
                <Grid2 container direction='row' justifyContent='space-between' alignItems='center'>
                    <Grid2 item>
                        <Chip
                            variant="outlined"
                            label={`${event.currentParticipantCount} of ${event.maxParticipantCount}`}
                            color={labelColor}
                        />
                    </Grid2>
                    <Grid2 item>
                        <Button>Overview</Button>
                    </Grid2>
                </Grid2>
            </Stack>
        </Card>
    );
}

function SearchComponent({ categories, onResultsReceived }) {

    function handleSearch(e) {
        e.preventDefault();
        const formData = new FormData(e.currentTarget);
        const params = new URLSearchParams(formData);
        axiosClient
            .get(`events/search?${params}&PageNumber=0`)
            .then(response => response.data)
            .then(data => {
                console.log(data);
                onResultsReceived(data, params.toString());
            })
            .catch(err => {
                console.log(err);
            })
    }

    return (
        <Card style={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)", overflow: "visible" }} sx={{ mb: 2 }}>
            <form onSubmit={handleSearch}>
                <Grid2 container direction='row' justifyContent='space-between' alignItems='center' p={2} spacing={2}>
                    <Grid2 item size={6}>
                        <FormControl fullWidth>
                            <TextField
                                size="small"
                                name="TextQuery"
                                label="Text query..."
                            />
                            <FormHelperText>
                                Search this text in event name/description
                            </FormHelperText>
                        </FormControl>
                    </Grid2>
                    <Grid2 item size={6}>
                        <FormControl fullWidth>
                            <TextField
                                size="small"
                                name="PlaceQuery"
                                label="Place query"
                            />
                            <FormHelperText>
                                Search this text in event address
                            </FormHelperText>
                        </FormControl>
                    </Grid2>
                    <Grid2 item size={3}>
                        <FormControl fullWidth>
                            <TextField
                                type="date"
                                size="small"
                                name="SearchFromDate"
                            />
                            <FormHelperText>
                                Search events from this date
                            </FormHelperText>
                        </FormControl>
                    </Grid2>
                    <Grid2 item size={3}>
                        <FormControl fullWidth>
                            <TextField
                                type="date"
                                size="small"
                                name="SearchUntilDate"
                            />
                            <FormHelperText>
                                Search events until this date
                            </FormHelperText>
                        </FormControl>

                    </Grid2>
                    <Grid2 item size={6}>
                        <FormControl fullWidth>
                            <Select isMulti options={categories} name="Categories" />
                            <FormHelperText>
                                Search events until this date
                            </FormHelperText>
                        </FormControl>
                    </Grid2>
                    <Grid2 item>
                        <Button type="submit">Search</Button>
                    </Grid2>
                </Grid2>
            </form>

        </Card>
    );
}

export default EventCatalogPage;
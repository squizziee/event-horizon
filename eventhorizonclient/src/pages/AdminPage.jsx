import { useContext, useEffect, useState } from "react";
import { UserContext } from "../App";
import { Backdrop, Box, Button, Card, CardActions, CardMedia, Chip, CircularProgress, Container, FormControl, FormHelperText, Grid2, IconButton, Pagination, Stack, Tab, TextField, Typography } from "@mui/material";
import { TabContext, TabList, TabPanel } from "@mui/lab";
import axiosClient from "../tools/axiosConfig";
import truncateDescription, { truncateName } from "../tools/textFormatter";
import imagePlaceholder from "../misc/event_placeholder.bmp";
import { Delete, Settings, Visibility } from "@mui/icons-material";
import Select from "react-select";
import Dropzone from "react-dropzone";

function AdminPage() {
    const { user, setUser } = useContext(UserContext)
    const [tab, setTab] = useState("1")

    useEffect(() => {
        axiosClient({
            method: "GET",
            url: "events"
        })
    })

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
            <TabContext value={tab}>
                <Box>
                    <TabList onChange={handleTabChange} aria-label="lab API tabs example" centered>
                        <Tab label="Categories" value="1" />
                        <Tab label="Events" value="2" />
                        <Tab label="Entires" value="3" />
                    </TabList>
                    <TabPanel value="1">

                    </TabPanel>
                    <TabPanel value="2">
                        <EventTab />
                    </TabPanel>
                    <TabPanel value="3">

                    </TabPanel>
                </Box>
            </TabContext>

        </Container>
    );
}

function EventTab() {
    const [pageNumber, setPageNumber] = useState(0)
    const [totalPages, setTotalPages] = useState(0)
    const [events, setEvents] = useState()

    const [addDialogOpened, setAddDialogOpened] = useState(false);
    const [editDialogOpened, setEditDialogOpened] = useState(false);

    function changePage(e, value) {
        console.log(value);

        setPageNumber(value - 1)
    }

    function openAddDialog() {
        setAddDialogOpened(true)
    }

    function closeAddDialog() {
        setAddDialogOpened(false)
    }

    function openEditDialog() {
        setEditDialogOpened(true)
    }

    function closeEditDialog() {
        setEditDialogOpened(false)
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
        <Stack direction="column">
            <Button onClick={openAddDialog}>Add event</Button>
            <Backdrop open={addDialogOpened} sx={{ zIndex: 99999 }}>
                <AddEventCard closeDialog={closeAddDialog} />
                <Button onClick={closeAddDialog}>Add event</Button>
            </Backdrop>
            {
                events ?
                    <Grid2 container spacing={2} justifyContent={'center'}>
                        {
                            events.map((e, index) => (
                                <Grid2 size={12} item key={index}>
                                    <EventListComponent event={e} />
                                </Grid2>
                            ))
                        }


                    </Grid2>
                    :
                    <CircularProgress />
            }
            <Grid2 container justifyContent={'center'}>
                <Grid2 item>
                    <Pagination style={{ paddingTop: "50px" }} count={totalPages} page={pageNumber + 1} onChange={changePage} />
                </Grid2>

            </Grid2>
        </Stack>

    );
}

function AddEventCard({ closeDialog }) {
    const [categories, setCategories] = useState()
    const [files, setFiles] = useState([])

    useEffect(() => {
        axiosClient({
            method: "GET",
            url: "category?DoNotPaginate=true&PageNumber=0"
        })
            .then(response => response.data)
            .then(data => {
                var mapped = data.categories.map(c => ({ value: c.id, label: c.name }))
                setCategories(mapped);
            })
            .catch(err => {
                console.log(err);
            })
    }, [])

    function handleAddEvent(e) {
        e.preventDefault();

        const formData = new FormData(e.currentTarget);

        for (const image of files) {
            formData.append("AttachedImages", image);
        }
        console.log(formData);
        axiosClient({
            method: "POST",
            url: "events",
            data: formData,
            headers: { "Content-Type": "multipart/form-data" }
        })
            .then(_ => {
                closeDialog();
            })
            .catch(err => {
                console.log(err);
            })
    }

    return (
        categories ?
            <Container maxWidth="sm">
                <Card style={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)", overflow: "visible" }}>
                    <form onSubmit={handleAddEvent}>
                        <Stack direction="column" p={2} spacing={1}>
                            <FormControl fullWidth>
                                <Select name="CategoryId" options={categories} required />
                                <FormHelperText>
                                    Event category
                                </FormHelperText>
                            </FormControl>
                            <FormControl fullWidth>
                                <TextField
                                    sx={{ zIndex: 0 }}
                                    required
                                    size="small"
                                    name="Name"
                                    label="Name"
                                />
                                <FormHelperText>
                                    Event name
                                </FormHelperText>
                            </FormControl>
                            <FormControl fullWidth>
                                <TextField
                                    sx={{ zIndex: 0 }}
                                    required
                                    multiline
                                    rows={4}
                                    size="small"
                                    name="Description"
                                    label="Description"
                                />
                                <FormHelperText>
                                    Event description
                                </FormHelperText>
                            </FormControl>
                            <FormControl fullWidth>
                                <TextField
                                    sx={{ zIndex: 0 }}
                                    required
                                    multiline
                                    rows={2}
                                    size="small"
                                    name="Address"
                                    label="Address"
                                />
                                <FormHelperText>
                                    Event address
                                </FormHelperText>
                            </FormControl>
                            <FormControl fullWidth>
                                <TextField
                                    sx={{ zIndex: 0 }}
                                    required
                                    type="date"
                                    size="small"
                                    name="Date"
                                />
                                <FormHelperText>
                                    Event date
                                </FormHelperText>
                            </FormControl>
                            <FormControl fullWidth>
                                <TextField
                                    sx={{ zIndex: 0 }}
                                    required
                                    type="time"
                                    size="small"
                                    name="Time"
                                />
                                <FormHelperText>
                                    Event time (UTC)
                                </FormHelperText>
                            </FormControl>
                            <FormControl fullWidth>
                                <TextField
                                    sx={{ zIndex: 0 }}
                                    required
                                    type="number"
                                    min={1}
                                    size="small"
                                    label="Number"
                                    name="MaxParticipantCount"
                                />
                                <FormHelperText>
                                    Maximum participants allowed in event
                                </FormHelperText>
                            </FormControl>
                            <FormControl fullWidth>
                                <Box sx={{ border: "0.5px solid rgba(0, 0, 0, .20)", borderRadius: "4px", padding: "0 20px", cursor: "pointer" }}>
                                    <Dropzone maxFiles={5} onDrop={acceptedFiles => setFiles(acceptedFiles)}>
                                        {({ getRootProps, getInputProps }) => (
                                            <section>
                                                <div {...getRootProps()}>
                                                    <input {...getInputProps()} />
                                                    <p>Click to select images ({files.length} selected)</p>
                                                </div>
                                            </section>
                                        )}
                                    </Dropzone>
                                </Box>

                                <FormHelperText>
                                    Event images (up to 5)
                                </FormHelperText>
                            </FormControl>

                        </Stack>

                        <CardActions sx={{ padding: 2 }}>
                            <Button type="submit">Proceed</Button>
                            <Button color="error">Cancel</Button>
                        </CardActions>
                    </form>
                </Card>
            </Container>

            :
            <CircularProgress />
    );
}

function EventListComponent({ event, onEditButtonPressed, onDeleteButtonPressed }) {
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
            <Stack direction="column" spacing={0.5} p={2} useFlexGap >
                <Stack direction="row" spacing={2} useFlexGap alignItems='center'>
                    <Typography variant="h5">{truncateName(event.name)}</Typography>
                    <Chip
                        size="small"
                        variant="filled"
                        label={`${event.category.name}`}
                        color='default'
                    />
                </Stack>
                <Typography variant="body2" sx={{ color: "GrayText" }}>{truncateDescription(event.description)}</Typography>
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
                        <IconButton onClick={() => window.location.href = `/event/${event.id}`}>
                            <Delete style={{ fontSize: 20 }} />
                        </IconButton>
                        <IconButton onClick={() => window.location.href = `/event/${event.id}`}>
                            <Settings style={{ fontSize: 20 }} />
                        </IconButton>
                        <IconButton onClick={() => window.location.href = `/event/${event.id}`}>
                            <Visibility style={{ fontSize: 20 }} />
                        </IconButton>
                    </Grid2>
                </Grid2>
            </Stack>
        </Card>
    );
}

export default AdminPage;
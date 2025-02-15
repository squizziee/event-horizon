import { useContext, useEffect, useState } from "react";
import { UserContext } from "../App";
import { Button, Card, Chip, Container, Grid2, Stack, Typography } from "@mui/material";
import axiosClient from "../tools/axiosConfig";
import formatTimestamp from "../tools/dateFormatter";

function ProfilePage() {
    const { user, setUser } = useContext(UserContext)
    const [entries, setEntries] = useState()

    useEffect(() => {
        axiosClient({
            method: "GET",
            url: "entries/event/user"
        })
            .then(response => response.data)
            .then(data => {
                setEntries(data.entries)
                console.log(data.entries);

            })
    }, [])

    function handleUnbookEntry(entryId) {
        axiosClient({
            method: "DELETE",
            url: `entries/${entryId}`
        })
            .catch(err => {
                console.log(err);
            })
    }

    return (
        <Container maxWidth="md" sx={{ marginTop: "50px" }}>
            <Stack direction="column" p={2}>
                <Typography variant="h2" sx={{ mb: 8, textAlign: "center" }}>Event entries</Typography>
                <Stack direction="column" spacing={1.5}>
                    {
                        entries ?
                            <div>
                                {
                                    entries.map((e, index) => (
                                        <EntryCard entry={e} handleUnbookEntry={handleUnbookEntry} key={index} />
                                    ))
                                }
                            </div>
                            :
                            <div></div>
                    }
                </Stack>
            </Stack>
        </Container>
    );
}

function EntryCard({ entry, handleUnbookEntry }) {
    console.log(entry);

    var labelColor = entry.event.currentParticipantCount == entry.event.maxParticipantCount ?
        "error" : "primary";

    return (
        <Card sx={{ boxShadow: "0 0 20px rgba(0, 0, 0, .075)", p: 2, mb: 2 }}>
            <Typography variant="caption">Applied on {formatTimestamp(entry.submissionDate)}</Typography>
            <Stack direction="column" spacing={0.5} useFlexGap>
                <Stack direction="row" spacing={2} useFlexGap alignItems='center'>
                    <Typography variant="h5">{entry.event.name}</Typography>
                </Stack>

                <Typography variant="body2" sx={{ color: "GrayText" }}>{entry.event.description}</Typography>
                <Typography variant="body2" sx={{ color: "GrayText" }}>Address: {entry.event.address}</Typography>
                <Grid2 container direction='row' justifyContent='space-between' alignItems='center'>
                    <Grid2 item>
                        <Chip
                            variant="outlined"
                            label={`${entry.event.currentParticipantCount} of ${entry.event.maxParticipantCount}`}
                            color={labelColor}
                        />
                    </Grid2>
                    <Grid2 item>
                        <Button color="error" sx={{ mr: 1 }} onClick={() => handleUnbookEntry(entry.id)}>Unbook entry</Button>
                        <Button variant="contained" disableElevation onClick={() => window.location.href = `event/${entry.event.id}`}>Overview</Button>
                    </Grid2>
                </Grid2>
            </Stack>
        </Card>
    );
}

export default ProfilePage;
function formatErrors(axiosError) {
    try {
        const errs = axiosError.response.data.title
            .split("--")
            .map(e => (e.trim()))
        return errs.join("\n")
    } catch (e) {
        return "Success!"
    }

}

export default formatErrors
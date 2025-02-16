export default function truncateDescription(text) {
    if (text.length < 100) return text;
    return text.substring(0, 100) + "..."
}

export function truncateName(text) {
    if (text.length < 20) return text;
    return text.substring(0, 20) + "..."
}
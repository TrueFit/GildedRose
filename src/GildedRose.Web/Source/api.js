export function add(name, category, quality, sellIn) {
    return new Promise((resolve, reject) => {
        fetch("/api/inventoryItems/add", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name, category, quality, sellIn })
        }).then(response => {
            if (response.ok) {
                resolve(response.json());
            } else {
                reject(new Error(response.statusText));
            }
        });
    });
}

export function getAll() {
    return new Promise((resolve, reject) => {
        fetch("/api/inventoryItems", { method: "GET" })
            .then(response => {
                if (response.ok) {
                    resolve(response.json());
                } else {
                    reject(new Error(response.statusText));
                }
            });
    });
}

export function nextDay() {
    return new Promise((resolve, reject) => {
        fetch("/api/inventoryItems/nextDay", { method: "POST" })
            .then(response => {
                if (response.ok) {
                    resolve(response.json());
                } else {
                    reject(new Error(response.statusText));
                }
            });
    });
}

export function removeItem(itemId) {
    return new Promise((resolve, reject) => {
        fetch(`/api/inventoryItems/${itemId}`, { method: "DELETE" })
            .then(response => {
                if (response.ok) {
                    resolve();
                } else {
                    reject(new Error(response.statusText));
                }
            });
    });
}

export function reset() {
    return new Promise((resolve, reject) => {
        fetch("/api/inventoryItems/reset", { method: "POST" })
            .then(response => {
                if (response.ok) {
                    resolve();
                } else {
                    reject(new Error(response.statusText));
                }
            });
    });
}
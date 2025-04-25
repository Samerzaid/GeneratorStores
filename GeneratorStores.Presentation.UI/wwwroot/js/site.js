window.wishlist = {
    get: () => JSON.parse(localStorage.getItem("wishlist") || "[]"),
    count: () => JSON.parse(localStorage.getItem("wishlist") || "[]").length,
    add: (id) => {
        const set = new Set(window.wishlist.get());
        set.add(id);
        localStorage.setItem("wishlist", JSON.stringify([...set]));
    },
    remove: (id) => {
        const set = new Set(window.wishlist.get());
        set.delete(id);
        localStorage.setItem("wishlist", JSON.stringify([...set]));
    },
    clear: () => localStorage.removeItem("wishlist")
};

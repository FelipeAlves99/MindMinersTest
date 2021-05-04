import React from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";

import UploadFile from "./pages/UploadFile";
import DownloadAnyFile from "./pages/DownloadPastFiles";

export default function Routes() {
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/" exact component={UploadFile} />
                <Route path="/filelist" component={DownloadAnyFile} />
            </Switch>
        </BrowserRouter>
    );
}
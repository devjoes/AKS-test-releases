import { apiUrl } from './settings';

export const get = async (path) => {
    const req = await fetch(getUrl(path));
    return await req.json();
};

export const put = async (path, data) => await send(path, data, 'PUT');

export const post = async (path, data) => await send(path, data, 'POST');


const getUrl = path =>  `${apiUrl}/${path.replace(/^\//, '')}`;

const send = async (path, data, method) => {
    const req = await fetch(getUrl(path), {
        method,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
      });
    const text = await req.text();
    if (text.length > 0){
        return await req.json();
    }
};
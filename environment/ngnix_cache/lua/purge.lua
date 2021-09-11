function file_exists(name)
        local f = io.open(name, "r")
        if f~=nil then io.close(f) return true else return false end
end

function delete(filename)
        if (file_exists(filename)) then
                os.remove(filename)
        end
end

local hash = ngx.md5(ngx.var.request_uri)
local length = string.len(hash)
local last_char = string.sub(hash, -1)
local cache_file = ngx.var.cache_dir .. "/" .. last_char .. "/" .. hash

-- ngx.say(ngx.var.request_uri)
-- ngx.say(hash)
-- ngx.say(length)
-- ngx.say(last_char)
-- ngx.say(cache_file)


delete(cache_file)
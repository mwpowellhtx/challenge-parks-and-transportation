#include "strutil.h"

namespace challenges {

    void split::push_back(std::vector<std::string> & result, std::string const & s) {
        if (_option == remove_empty_entries && s.empty()) return;
        result.push_back(s);
    }

    split::split(split_option option)
        : _option(option) {
    }

    std::vector<std::string> split::operator()(std::string const & s, std::string const & d) {
        std::vector<std::string> result;
        std::string::size_type current = 0, found;
        while ((found = s.find_first_of(d, current)) != std::string::npos) {
            push_back(result, std::string(s, current, found - current));
            current = found + 1;
        }
        push_back(result, std::string(s, current, s.size() - current));
        return result;
    }
}
